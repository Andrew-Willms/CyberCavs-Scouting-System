using GameMakerWpf.Domain.EditingData;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;




public class BooleanDataFieldEditor : DataFieldTypeEditor {

	private GameEditor GameEditor { get; }

	public SingleInput<bool, bool, ErrorSeverity> InitialValue { get; }

	public BooleanDataFieldEditor(GameEditor gameEditor, BooleanDataFieldEditingData initialValues) {

		GameEditor = gameEditor;

		// todo maybe extract these lambda functions to functions in the same place as the other converters and inverters
		InitialValue = new SingleInputCreator<bool, bool, ErrorSeverity> {
			Converter = input => (input, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			Inverter = output => (output, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();

		InitialValue.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
	}

}