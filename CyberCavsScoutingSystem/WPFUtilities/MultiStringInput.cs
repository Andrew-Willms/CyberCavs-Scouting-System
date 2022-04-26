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

	public ReadOnlyCollection<string> InputComponentNames { get; }

	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }

	private MultiStringInputCovalidator<TTargetType, TSeverityEnum> Covalidator { get; }

	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _CovalidationErrors = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> CovalidationErrors {

		// TODO: .Net 7.0 remove backing field
		get => _CovalidationErrors;

		private set {
			_CovalidationErrors = value;
			OnErrorsChanged();
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
	public string this[string inputComponentName] {
		
		get {

			if (InputComponentNames.Contains(inputComponentName) == false) {
				throw new ArgumentException($"This {nameof(MultiStringInput<TTargetType, TSeverityEnum>)} object does not contain the {nameof(inputComponentName)} \"{inputComponentName}\"");
			}

			return StringInputs[inputComponentName].InputString;
		}

		set {

			if (InputComponentNames.Contains(inputComponentName) == false) {
				throw new ArgumentException($"This {nameof(MultiStringInput<TTargetType, TSeverityEnum>)} object does not contain the {nameof(inputComponentName)} \"{inputComponentName}\"");
			}

			StringInputs[inputComponentName].InputString = value;

			CovalidateInput();

			//OnSubPropertyChanged();
			OnPropertyChanged($"Item[{inputComponentName}]"); // Could try Binding.Indexer name and injecting the inputComponentName
		}
	}



	public MultiStringInput(MultiStringInputCovalidator<TTargetType, TSeverityEnum> covalidator,
		params (string inputComponentName, IStringInput<TSeverityEnum> stringInput)[] inputComponents) {

		Covalidator = covalidator;

		InputComponentNames = inputComponents.Select(x => x.inputComponentName).ToList().AsReadOnly();
		StringInputs = new(inputComponents.ToDictionary(x => x.inputComponentName, x => x.stringInput));

		foreach (IStringInput<TSeverityEnum> inputString in StringInputs.Values) {

			inputString.PropertyChanged += OnComponentInputChanged;

		}

		CovalidateInput();
	}


	protected void OnComponentInputChanged(object? sender, PropertyChangedEventArgs e) {

		//e.PropertyName;

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



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	//protected void OnSubPropertyChanged() {
	//	string test = $"Item[{propertyIdentifier}]"; // Could try Binding.Indexer name and injecting the propertyIdentifier
	//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	//}

	protected void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CovalidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComponentValidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
	}
}