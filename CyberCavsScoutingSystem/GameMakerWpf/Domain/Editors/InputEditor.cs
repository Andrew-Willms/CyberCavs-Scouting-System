using System.Collections.Generic;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors; 



public class InputEditor {

	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> DataFieldName { get; }

	public SingleInput<string, string, ErrorSeverity> InputText { get; }

	public InputEditor(GameEditor gameEditor, InputEditingData initialValues) {

		GameEditor = gameEditor;

		DataFieldName = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = InputValidators.DataFieldNameConverter,
			Inverter = InputValidators.DataFieldNameInverter,
			InitialInput = initialValues.DataFieldName
		}.AddValidationRule<IEnumerable<DataFieldEditor>>(InputValidators.DataFieldNameValidator_DataFieldOfNameExists, () => GameEditor.DataFields, 
			true, GameEditor.DataFieldNameChanged, GameEditor.DataFieldTypeChanged)
		.CreateSingleInput();

		InputText = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = InputValidators.InputTextConverter,
			Inverter = InputValidators.InputTextInverter,
			InitialInput = initialValues.InputText
		}.AddValidationRule(InputValidators.InputTextValidator_Length)
		.CreateSingleInput();
	}

	public InputEditingData ToEditingData() {

		return new() {
			DataFieldName = DataFieldName.InputObject,
			InputText = InputText.InputObject
		};
	}

}