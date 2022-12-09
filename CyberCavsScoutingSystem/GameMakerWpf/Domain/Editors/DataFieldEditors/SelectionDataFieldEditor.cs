using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.SimpleEvent;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



public class SelectionDataFieldEditor : DataFieldTypeEditor {

	private GameEditor GameEditor { get; }

	private ObservableCollection<SingleInput<string, string, ErrorSeverity>> _Options { get; } = new();
	public ReadOnlyObservableCollection<SingleInput<string, string, ErrorSeverity>> Options => new(_Options);

	public Event OptionNameChanged { get; } = new();

	public SelectionDataFieldEditor(GameEditor gameEditor, SelectionDataFieldEditingData initialValues) {

		GameEditor = gameEditor;

		initialValues.OptionNames.Foreach(AddOption);
	}

	public void AddOption(string optionName) {

		SingleInput<string, string, ErrorSeverity> option = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = SelectionDataFieldValidator.OptionNameConverter,
			Inverter = SelectionDataFieldValidator.OptionNameInverter,
			InitialInput = optionName
		}.AddValidationRule(SelectionDataFieldValidator.OptionNameValidator_Length)
		.AddValidationRule<IEnumerable<SingleInput<string, string, ErrorSeverity>>>(
			SelectionDataFieldValidator.OptionNameValidator_Uniqueness, () => Options, false, OptionNameChanged)
		.CreateSingleInput();

		option.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);

		_Options.Add(option);

		OptionNameChanged.SubscribeTo(option.OutputObjectChanged);
		OptionNameChanged.Invoke();
		GameEditor.AnythingChanged.Invoke();
	}

	public Result<RemoveOptionError> RemoveOption(SingleInput<string, string, ErrorSeverity> option) {

		if (!_Options.Remove(option)) {
			return new RemoveOptionError { ErrorType = RemoveOptionError.Types.OptionNotFound };
		}

		OptionNameChanged.UnsubscribeFrom(option.OutputObjectChanged);
		OptionNameChanged.Invoke();

		option.OutputObjectChanged.UnSubscribe(GameEditor.AnythingChanged.Invoke);
		GameEditor.AnythingChanged.Invoke();

		return new Success();
	}

	public class RemoveOptionError : Error<RemoveOptionError.Types> {

		public enum Types {
			OptionNotFound
		}

	}

}