using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;

public class IntegerDataFieldEditor : DataFieldTypeEditor {

	public SingleInput<int, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<int, string, ErrorSeverity> MinValue { get; }

	public SingleInput<int, string, ErrorSeverity> MaxValue { get; }



	public IntegerDataFieldEditor(IntegerDataFieldEditingData initialValues) {

		InitialValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();

		MinValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.MinValue
		}.CreateSingleInput();

		MaxValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.MaxValue
		}.CreateSingleInput();
	}

}