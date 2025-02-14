using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Optional;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace CCSSDomain.DataCollectors;



public abstract class DataField : INotifyPropertyChanged {

	public DataFieldSpec Specification { get; }

	public string Name => Specification.Name;

	public abstract object BaseValue { get; }

	public readonly Event OnValueChange = new();

	public abstract List<string> Errors { get; }

	protected DataField(DataFieldSpec specification) {
		Specification = specification;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public class BooleanDataField : DataField {

	public new BooleanDataFieldSpec Specification { get; }

	public bool Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
			OnPropertyChanged(nameof(Errors));
		}
	}

	public override object BaseValue => Value;

	public override List<string> Errors => [];

	public BooleanDataField(BooleanDataFieldSpec specification) : base(specification) {
		Value = specification.InitialValue;
		Specification = specification;
	}

}



public class TextDataField : DataField {

	public new TextDataFieldSpec Specification { get; }

	public string Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
			OnPropertyChanged(nameof(Errors));
		}
	}

	public override object BaseValue => Value;

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (Value == string.Empty &&
			    (Specification.MustNotBeEmpty ||
			     Specification is { MustNotBeInitialValue: true, InitialValue: "" })) {

				errors.Add($"The data field \"{Name}\" is empty.");
			}

			if (Specification.MustNotBeInitialValue && Value == Specification.InitialValue) {

				errors.Add($"The data field \"{Name}\" must not be the default value {Specification.InitialValue}.");
			}

			return errors;
		}
	}

	public TextDataField(TextDataFieldSpec specification) : base(specification) {
		Value = specification.InitialValue;
		Specification = specification;
	}

}



public class IntegerDataField : DataField {

	public new IntegerDataFieldSpec Specification { get; }

	public int Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
			OnPropertyChanged(nameof(Errors));
		}
	}

	public override object BaseValue => Value;

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (Value > Specification.MaxValue) {
				errors.Add($"The data field \"{Name}\" ist set to {Value} which is greater than it's maximum value of {Specification.MaxValue}.");
			}

			if (Value < Specification.MinValue) {
				errors.Add($"The data field \"{Name}\" ist set to {Value} which is less than it's minimum value of {Specification.MinValue}.");
			}

			return errors;
		}
	}

	public IntegerDataField(IntegerDataFieldSpec specification) : base(specification) {
		Value = specification.InitialValue;
		Specification = specification;
	}

}



public class SelectionDataField : DataField {

	public new SelectionDataFieldSpec Specification { get; }

	public Optional<string> SelectedOption {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(SelectedOption));
		}
	} = Optional.NoValue;

	public Optional<string> Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
			OnPropertyChanged(nameof(Errors));
		}
	}

	public override object BaseValue => Value;

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (!Specification.RequiresValue && SelectedOption == Optional<string>.NoValue) {
				errors.Add($"The data field \"{Name}\" requires a value value.");
			}

			if (SelectedOption != Optional.NoValue && !Specification.Options.Contains(SelectedOption.Value)) {
				errors.Add($"The data field \"{Name}\" does not contain the specified value '{SelectedOption.Value}'");
			}

			return errors;
		}
	}

	public SelectionDataField(SelectionDataFieldSpec specification) : base(specification) {
		Value = specification.InitialValue;
		Specification = specification;
	}

}