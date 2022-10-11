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

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = DataFieldValidator.NameConverter,
			Inverter = DataFieldValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddOnChangeValidator(DataFieldValidator.NameValidator_Length)
		.AddTriggeredValidator(DataFieldValidator.NameValidator_Uniqueness, () => EditingData.DataFields, EditingData.DataFieldNameChanged)
		.CreateSingleInput();
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

			Options.Add(new SingleInputCreator<string, string, ErrorSeverity> {
					Converter = SelectionDataFieldValidator.OptionNameConverter,
					Inverter = SelectionDataFieldValidator.OptionNameInverter,
					InitialInput = optionName
				}.AddOnChangeValidator(SelectionDataFieldValidator.OptionNameValidator_Length)
				.AddTriggeredValidator(SelectionDataFieldValidator.OptionNameValidator_Uniqueness, () => Options, EditingData.DataFieldNameChanged)
				.CreateSingleInput()
			);
		}
	}

}



public class IntegerDataFieldEditingData : DataFieldEditingData {

	public SingleInput<int, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<int, string, ErrorSeverity> MinValue { get; }

	public SingleInput<int, string, ErrorSeverity> MaxValue { get; }



	public IntegerDataFieldEditingData(GameEditingData editingData, IntegerDataField initialValues) : base(editingData, initialValues) {

		InitialValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();

		MinValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();

		MaxValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();
	}

}