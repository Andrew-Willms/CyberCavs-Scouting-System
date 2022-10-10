using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain; 



public abstract class DataFieldEditingData {

	protected GameEditingData EditingData { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }



	protected DataFieldEditingData(GameEditingData editingData, DataField initialValues) {

		EditingData = editingData;

		Name = new(DataFieldValidator.NameConverter, DataFieldValidator.NameInverter, initialValues.Name,
			new ValidationSet<string, ErrorSeverity>(DataFieldValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<DataFieldEditingData>, ErrorSeverity>(
				DataFieldValidator.NameValidator_Uniqueness,
				() => EditingData.DataFields.Where(x => x != this),
				EditingData.DataFieldNameChanged)
		);
	}

	public static DataFieldEditingData DataFieldEditingDataFromDataField(DataField dataField, GameEditingData gameEditingData) {

		return dataField switch {
			TextDataField textDataField => new TextDataFieldEditingData(gameEditingData, textDataField),
			SelectionDataField selectionDataField => new SelectionDataFieldEditingData(gameEditingData, selectionDataField),
			IntegerDataField integerDataField => new IntegerDataFieldEditingData(gameEditingData, integerDataField),
			_ => throw new ShouldMatchOtherCaseException()
		};
	}

}

public class TextDataFieldEditingData : DataFieldEditingData {

	public TextDataFieldEditingData(GameEditingData editingData, TextDataField initialValues) : base(editingData, initialValues) { }
}

public class SelectionDataFieldEditingData : DataFieldEditingData {

	public ObservableCollection<SingleInput<string, string, ErrorSeverity>> Options { get; } = new();

	public SelectionDataFieldEditingData(GameEditingData editingData, SelectionDataField initialValues) : base(editingData, initialValues) {

		foreach (string optionName in initialValues.OptionNames) {

			Options.Add(new(
				converter: SelectionDataFieldValidator.OptionNameConverter,
				inverter: SelectionDataFieldValidator.OptionNameInverter,
				initialInput: optionName,
				validationSets: new IValidationSet<string, ErrorSeverity>[] {
					new ValidationSet<string, IEnumerable<SingleInput<string, string, ErrorSeverity>>, ErrorSeverity>(
						validator: SelectionDataFieldValidator.OptionNameValidator_Uniqueness,
						validationParameterGetter: () => Options, 
						validationEvents: EditingData.DataFieldNameChanged),
					new ValidationSet<string, ErrorSeverity>(
						validator: SelectionDataFieldValidator.OptionNameValidator_Length),
				}
			));
		}
	}

}

public class IntegerDataFieldEditingData : DataFieldEditingData {

	public SingleInput<int, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<int, string, ErrorSeverity> MinValue { get; }

	public SingleInput<int, string, ErrorSeverity> MaxValue { get; }



	public IntegerDataFieldEditingData(GameEditingData editingData, IntegerDataField initialValues) : base(editingData, initialValues) {

		InitialValue = new(
			converter: IntegerDataFieldValidator.IntegerValueConverter,
			inverter: IntegerDataFieldValidator.IntegerValueInverter,
			initialInput: initialValues.InitialValue.ToString()
		);

		MinValue = new(
			converter: IntegerDataFieldValidator.IntegerValueConverter,
			inverter: IntegerDataFieldValidator.IntegerValueInverter,
			initialInput: initialValues.MinValue.ToString()
		);

		MaxValue = new(
			converter: IntegerDataFieldValidator.IntegerValueConverter,
			inverter: IntegerDataFieldValidator.IntegerValueInverter,
			initialInput: initialValues.MaxValue.ToString()
		);
	}

}