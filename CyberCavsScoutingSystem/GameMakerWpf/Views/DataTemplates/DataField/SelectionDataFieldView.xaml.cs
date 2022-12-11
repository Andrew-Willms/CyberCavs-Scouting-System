using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using CCSSDomain;
using GameMakerWpf.AppManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.Validation.Inputs;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.DataTemplates.DataField;



public partial class SelectionDataFieldView : AppManagerDependent, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();



	private SelectionDataFieldEditor? Editor => App.Manager.SelectedDataField?.DataFieldTypeEditor as SelectionDataFieldEditor;
	
	[DependsOn(nameof(AppManager.GameEditor))]
	[DependsOn(nameof(AppManager.SelectedDataField))]
	public ObservableList<SingleInput<string, string, ErrorSeverity>, string>? Options => Editor?.Options;

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
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {

		if (Options is null) {
			throw new InvalidOperationException($"You should not be able to remove an option when the {nameof(Editor)} is null.");
		}

		Options.Add("Test");
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {

		if (SelectedOption is null) {
			throw new InvalidOperationException("You should not be able to press the remove button while there is no Option selected.");
		}

		if (Options is null) {
			throw new InvalidOperationException($"You should not be able to remove an option when the {nameof(Editor)} is null.");
		}

		Result<ListRemoveError> result = Options.Remove(SelectedOption);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case ListRemoveError { ErrorType: ListRemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveOptionError.OptionNotFoundCaption, ErrorData.RemoveOptionError.OptionNotFoundMessage);
				return;

			// TODO: make new error message for this case
			case ListRemoveError { ErrorType: ListRemoveError.Types.OtherFailure }:
				ErrorPresenter.DisplayError("todo", "todo");
				return;

			default:
				throw new UnreachableException();
		}
	}

	private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;
	
	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}