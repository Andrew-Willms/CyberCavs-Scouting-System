using System.Collections.Generic;
using System.ComponentModel;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.SimpleEvent;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



public class SelectionDataFieldEditor : DataFieldTypeEditor {

	private GameEditor GameEditor { get; }

	public ObservableList<SingleInput<string, string, ErrorSeverity>, string> Options { get; }

	public SingleInput<bool, bool, ErrorSeverity> RequiresValue { get; }

	private Event OptionNameChanged { get; } = new();

	public SelectionDataFieldEditor(GameEditor gameEditor, SelectionDataFieldEditingData initialValues) {

		GameEditor = gameEditor;

		Options = new() {

			Adder = optionName => {
				return new SingleInputCreator<string, string, ErrorSeverity> {
					Converter = SelectionDataFieldValidator.OptionNameConverter,
					Inverter = SelectionDataFieldValidator.OptionNameInverter,
					InitialInput = optionName
				}.AddValidationRule(SelectionDataFieldValidator.OptionNameValidator_Length)
				.AddValidationRule<IEnumerable<SingleInput<string, string, ErrorSeverity>>>(
					SelectionDataFieldValidator.OptionNameValidator_Uniqueness, () => Options!, false, OptionNameChanged)
				.CreateSingleInput();
			},

			OnAdd = optionInput => {
				OptionNameChanged.SubscribeTo(optionInput.OutputObjectChanged);
				OptionNameChanged.Invoke();
				GameEditor.AnythingChanged.SubscribeTo(optionInput.OutputObjectChanged);
				GameEditor.AnythingChanged.Invoke();
			},

			OnRemove = optionInput => {
				OptionNameChanged.UnsubscribeFrom(optionInput.OutputObjectChanged);
				OptionNameChanged.Invoke();
				GameEditor.AnythingChanged.UnsubscribeFrom(optionInput.OutputObjectChanged);
				GameEditor.AnythingChanged.Invoke();
			}
		};

		RequiresValue = new SingleInputCreator<bool, bool, ErrorSeverity> {
			Converter = input => (input, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			Inverter = output => (output, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty),
			InitialInput = initialValues.RequiresValue
		}.CreateSingleInput();

		initialValues.OptionNames.Foreach(Options.Add);
	}

}