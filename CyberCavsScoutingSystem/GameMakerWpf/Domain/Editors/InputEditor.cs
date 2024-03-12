using System.Collections.Generic;
using CCSSDomain.GameSpecification;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors; 



public class InputEditor {

	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> DataFieldName { get; }

	public SingleInput<string, string, ErrorSeverity> Label { get; }

	public InputEditor(GameEditor gameEditor, InputEditingData initialValues) {

		GameEditor = gameEditor;

		DataFieldName = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = InputValidators.DataFieldNameConverter,
			Inverter = InputValidators.DataFieldNameInverter,
			InitialInput = initialValues.DataFieldName
		}.AddValidationRule<IEnumerable<DataFieldEditor>>(InputValidators.DataFieldNameValidator_DataFieldOfNameExists, () => GameEditor.DataFields, 
			true, GameEditor.DataFieldNameChanged, GameEditor.DataFieldTypeChanged)
		.CreateSingleInput();

		Label = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = InputValidators.InputTextConverter,
			Inverter = InputValidators.InputTextInverter,
			InitialInput = initialValues.Label
		}.AddValidationRule(InputValidators.InputTextValidator_Length)
		.CreateSingleInput();

		DataFieldName.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
		Label.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
	}



	public InputEditingData ToEditingData() {

		return new() {
			DataFieldName = DataFieldName.InputObject,
			Label = Label.InputObject
		};
	}



	public bool IsValid => DataFieldName.IsValid && Label.IsValid;

	public IEditorToGameSpecificationResult<InputSpec> ToGameSpecification() {

		if (!IsValid) {
			return new IEditorToGameSpecificationResult<InputSpec>.EditorIsInvalid();
		}

		return new IEditorToGameSpecificationResult<InputSpec>.Success { Value = new() {
			DataFieldName = DataFieldName.OutputObject.Value,
			Label = Label.OutputObject.Value
		}};
	}

}