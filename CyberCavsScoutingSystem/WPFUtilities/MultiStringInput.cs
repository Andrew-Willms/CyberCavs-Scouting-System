using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPFUtilities;



// A custom delegate used as the type for the InputValidator.
public delegate (TTargetType, ReadOnlyCollection<ValidationError<TSeverityEnum>>) MultiStringInputCovalidator<TTargetType, TSeverityEnum>
	(in ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> stringInputComponents) where TSeverityEnum : Enum;



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

	public TSeverityEnum CovalidationErrorLevel {

		get {

			List<TSeverityEnum> allErrorLevels = CovalidationErrors.Select(x => x.Severity).ToList();

			TSeverityEnum? returnValue = allErrorLevels.Any() ? allErrorLevels.Max() : default;

			if (returnValue is null) {
				throw new ArgumentException($"The default value of the {nameof(TSeverityEnum)} type parameter \"{typeof(TSeverityEnum)}\" is null.");
			}

			return returnValue;
		}
	}

	public TSeverityEnum OverallErrorLevel {

		get {

			List<TSeverityEnum> allErrorLevels = AllValidationErrors.Select(x => x.Severity).ToList();

			TSeverityEnum? returnValue = allErrorLevels.Any() ? allErrorLevels.Max() : default;

			if (returnValue is null) {
				throw new ArgumentException($"The default value of the {nameof(TSeverityEnum)} type parameter \"{typeof(TSeverityEnum)}\" is null.");
			}

			return returnValue;
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

		CovalidateInput();
	}

	private void CovalidateInput() {
		(TargetObject, CovalidationErrors) = Covalidator(StringInputs);
	}

	public void Validate() {

		foreach (IStringInput<TSeverityEnum> stringInput in StringInputs.Values) {
			stringInput.ValidateInput();
		}

		CovalidateInput();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnErrorsChanged() {

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CovalidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComponentValidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CovalidationErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverallErrorLevel)));
	}
}