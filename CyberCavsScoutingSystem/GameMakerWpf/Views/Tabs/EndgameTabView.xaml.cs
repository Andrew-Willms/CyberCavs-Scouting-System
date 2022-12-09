using System;
using System.ComponentModel;
using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.Tabs;



public partial class EndgameTabView : AppManagerDependent, INotifyPropertyChanged  {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.Manager.GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<InputEditor, InputEditingData> Inputs => GameEditor.EndgameTabInputs;

	private InputEditor? _SelectedInput;
	public InputEditor? SelectedInput {
		get => _SelectedInput;
		set {
			_SelectedInput = value;
			OnPropertyChanged(nameof(SelectedInput));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => App.Manager.SelectedDataField is not null;



	public EndgameTabView() {

		DataContext = this;

		InitializeComponent();
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {

		Inputs.Add(DefaultEditingDataValues.DefaultInputEditingData);
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {

		if (SelectedInput is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<ListRemoveError> result = Inputs.Remove(SelectedInput);

		switch (result.Resolve()) {
			
			case Success:
				return;

			// TODO replace this with an appropriate error messages
			case ListRemoveError { ErrorType: ListRemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAutoInputError.InputNotFoundCaption, ErrorData.RemoveAutoInputError.InputNotFoundMessage);
				return;

			// TODO replace this with an appropriate error messages
			case ListRemoveError { ErrorType: ListRemoveError.Types.OtherFailure }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAutoInputError.InputNotFoundCaption, ErrorData.RemoveAutoInputError.InputNotFoundMessage);
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



	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}
