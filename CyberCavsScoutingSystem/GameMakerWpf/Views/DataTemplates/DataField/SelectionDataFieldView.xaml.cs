using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CCSSDomain;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views.DataTemplates.DataField;



public partial class SelectionDataFieldView : UserControl, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();



	private SelectionDataFieldEditor? Editor => ApplicationManager.SelectedDataField?.DataFieldTypeEditor as SelectionDataFieldEditor;
	
	public ReadOnlyObservableCollection<SingleInput<string, string, ErrorSeverity>>? Options => Editor?.Options;

	private SingleInput<string, string, ErrorSeverity>? _SelectedOption;
	public SingleInput<string, string, ErrorSeverity>? SelectedOption {
		get => _SelectedOption;
		set {
			_SelectedOption = value;
			OnPropertyChanged(nameof(SelectedOption));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => SelectedOption is not null;



	public SelectionDataFieldView() {

		DataContext = this;

		InitializeComponent();

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);
		ApplicationManager.RegisterSelectedDataFieldChangeAction(EditorChanged);
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {

		if (Editor is null) {
			throw new InvalidOperationException($"You should not be able to remove an option when the {nameof(Editor)} is null.");
		}

		Editor.AddOption("Test");
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {

		if (SelectedOption is null) {
			throw new InvalidOperationException("You should not be able to press the remove button while there is no Option selected.");
		}

		if (Editor is null) {
			throw new InvalidOperationException($"You should not be able to remove an option when the {nameof(Editor)} is null.");
		}

		Result<SelectionDataFieldEditor.RemoveOptionError> result = Editor.RemoveOption(SelectedOption);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case SelectionDataFieldEditor.RemoveOptionError { ErrorType: SelectionDataFieldEditor.RemoveOptionError.Types.OptionNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveOptionError.OptionNotFoundCaption, ErrorData.RemoveOptionError.OptionNotFoundMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;
	
	private void OnPropertyChanged(string? propertyName = null) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

	private void EditorChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}