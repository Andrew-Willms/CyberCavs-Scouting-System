using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPFUtilities;



// A custom delegate used as the type for the InputValidator.
public delegate (TTargetType, ReadOnlyCollection<ValidationError<TSeverityEnum>>) MultiStringInputCovalidator<TTargetType, TSeverityEnum>
	(in TTargetType? value, in ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> stringInputComponents)
	where TSeverityEnum : Enum;



public class MultiStringInput<TTargetType, TSeverityEnum> : INotifyPropertyChanged where TSeverityEnum : Enum {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject {

		// TODO: .Net 7.0 remove backing field
		get => IsValid ? _TargetObject : default;
		private set => _TargetObject = value;
	}

	public ReadOnlyCollection<string> PropertyNames { get; }
	public ReadOnlyDictionary<string, PropertyInfo> PropertyInfos { get; }
	public ReadOnlyDictionary<string, MethodInfo> PropertySetters { get; }
	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }

	private MultiStringInputCovalidator<TTargetType, TSeverityEnum> Covalidator { get; }

	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _CovalidationErrors = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> CovalidationErrors {

		// TODO: .Net 7.0 remove backing field
		get => _CovalidationErrors;

		private set {
			_CovalidationErrors = value;
			OnErrorsListChanged();
		}
	}

	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ComponentValidationErrors =>
		StringInputs.Values.SelectMany(x => x.ValidationErrors).ToList().AsReadOnly();

	public ReadOnlyCollection<ValidationError<TSeverityEnum>> AllValidationErrors =>
		CovalidationErrors.Concat(ComponentValidationErrors).ToList().AsReadOnly();

	public bool IsValid => AllValidationErrors.Any() == false;

	public TSeverityEnum ErrorLevel {

		get {

			List<TSeverityEnum> allErrorLevels = AllValidationErrors.Select(x => x.Severity).ToList();

			TSeverityEnum? returnValue = allErrorLevels.Any() ? allErrorLevels.Max() : default;

			if (returnValue is null) {
				throw new ArgumentException($"The default value of the {nameof(TSeverityEnum)} type parameter \"{typeof(TSeverityEnum)}\" is null.");
			}

			return returnValue;
		}
	}



	// Test if changing the name still works with data binding.
	//[IndexerName("test indexer name")]
	public string this[string propertyIdentifier] {
		
		get {
			if (!PropertyNames.Contains(propertyIdentifier)) {
				throw new ArgumentException($"This StringInput object does not contain the propertyIdentifier \"{propertyIdentifier}\"");
			}
			return StringInputs[propertyIdentifier].InputString;
		}

		set {
			if (!PropertyNames.Contains(propertyIdentifier)) {
				throw new ArgumentException($"This StringInput object does not contain the propertyIdentifier \"{propertyIdentifier}\"");
			}

			StringInputs[propertyIdentifier].InputString = value;

			CovalidateInput();

			//OnSubPropertyChanged();
			OnPropertyChanged($"Item[{propertyIdentifier}]"); // Could try Binding.Indexer name and injecting the propertyIdentifier
		}
	}



	public MultiStringInput(MultiStringInputCovalidator<TTargetType, TSeverityEnum> covalidator,
		params (string propertyName, IStringInput<TSeverityEnum> stringInput)[] inputProperties) {

		Covalidator = covalidator;

		// I should probably validate the propertyNames here?

		PropertyInfos = new(inputProperties.ToDictionary(x => x.propertyName, x => GetAndAssertPropertyInfo(x.propertyName)));
		PropertySetters = new(inputProperties.ToDictionary(x => x.propertyName, x => GetAndAssertPropertySetters(x.propertyName)));
		PropertyNames = inputProperties.Select(x => x.propertyName).ToList().AsReadOnly();
		StringInputs = new(inputProperties.ToDictionary(x => x.propertyName, x => x.stringInput));

		CovalidateInput();
	}



	private void CovalidateInput() {
		(TargetObject, CovalidationErrors) = Covalidator(TargetObject, StringInputs);
	}

	public void Validate() {

		foreach (IStringInput<TSeverityEnum> stringInput in StringInputs.Values) {
			stringInput.ValidateInput();
		}

		CovalidateInput();
	}



	private static PropertyInfo GetAndAssertPropertyInfo(string propertyName) {

		Type targetType = typeof(TTargetType);
		PropertyInfo? propertyInfo = targetType.GetProperty(propertyName);

		if (propertyInfo is null) {
			throw new ArgumentException($"The target type \"{nameof(TTargetType)}\" does not have the property \"{propertyName}\".");
		}

		return propertyInfo;
	}

	private static MethodInfo GetAndAssertPropertySetters(string propertyName) {

		Type targetType = typeof(TTargetType);
		PropertyInfo? propertyInfo = targetType.GetProperty(propertyName);

		if (propertyInfo is null) {
			throw new ArgumentException($"The target type \"{nameof(TTargetType)}\" does not have the property \"{propertyName}\".");
		}

		MethodInfo? methodInfo = propertyInfo.SetMethod;

		if (methodInfo is null) {
			throw new ArgumentException($"The property \"{nameof(TTargetType)}\".\"{propertyName}\" does not have an accessible setter.");
		}

		return methodInfo;
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	//protected void OnSubPropertyChanged() {

	//	string test = $"Item[{propertyIdentifier}]"; // Could try Binding.Indexer name and injecting the propertyIdentifier

	//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	//}

	protected void OnErrorsListChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CovalidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
	}
}