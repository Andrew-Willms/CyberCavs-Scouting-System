using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



public class TextDataFieldEditor : DataFieldTypeEditor {

	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<bool, bool, ErrorSeverity> MustNotBeEmpty { get; }

	public SingleInput<bool, bool, ErrorSeverity> MustNotBeInitialValue { get; }

	public TextDataFieldEditor(GameEditor gameEditor, TextDataFieldEditingData initialValues) {

		GameEditor = gameEditor;

		InitialValue = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = TextDataFieldValidator.DefaultValueConverter,
			Inverter = TextDataFieldValidator.DefaultValueInverter,
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();

		// todo figure out if it makes sense for the inverters to also return a list of errors, how would inverter errors even be used?
		
		// todo maybe extract these lambda functions to functions in the same place as the other converters and inverters
		MustNotBeEmpty = new SingleInputCreator<bool, bool, ErrorSeverity> {
			Converter = input => (input, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			Inverter = output => (output, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			InitialInput = initialValues.MustNotBeEmpty
		}.CreateSingleInput();

		MustNotBeInitialValue = new SingleInputCreator<bool, bool, ErrorSeverity> {
			Converter = input => (input, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			Inverter = output => (output, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			InitialInput = initialValues.MustNotBeInitialValue
		}.CreateSingleInput();

		InitialValue.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
		MustNotBeEmpty.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
		MustNotBeInitialValue.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
	}

}