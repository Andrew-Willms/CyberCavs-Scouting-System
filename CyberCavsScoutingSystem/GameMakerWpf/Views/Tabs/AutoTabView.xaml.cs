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



public partial class AutoTabView : AppManagerDependent, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.Manager.GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<ButtonEditor, ButtonEditingData> Buttons => GameEditor.AutoButtons;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<InputEditor, InputEditingData> Inputs => GameEditor.AutoTabInputs;

	private ButtonEditor? _SelectedButton;
	public ButtonEditor? SelectedButton {
		get => _SelectedButton;
		set {
			_SelectedButton = value;
			OnPropertyChanged(nameof(SelectedButton));
			OnPropertyChanged(nameof(RemoveButtonButtonIsEnabled));
		}
	}

	public bool RemoveButtonButtonIsEnabled => SelectedButton is not null;

	private InputEditor? _SelectedInput;
	public InputEditor? SelectedInput {
		get => _SelectedInput;
		set {
			_SelectedInput = value;
			OnPropertyChanged(nameof(SelectedInput));
			OnPropertyChanged(nameof(RemoveInputButtonIsEnabled));
		}
	}

	public bool RemoveInputButtonIsEnabled => SelectedInput is not null;



	public AutoTabView() {

		DataContext = this;

		InitializeComponent();
	}



	private void AddButtonButton_Click(object sender, RoutedEventArgs e) {
		
		Buttons.Add(DefaultEditingDataValues.DefaultButtonEditingData);
	}

	private void RemoveButtonButton_Click(object sender, RoutedEventArgs e) {
		
		if (SelectedButton is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<ListRemoveError> result = Buttons.Remove(SelectedButton);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case ListRemoveError { ErrorType: ListRemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAutoButtonError.ButtonNotFoundCaption,ErrorData.RemoveAutoButtonError.ButtonNotFoundMessage);
				return;

			// TODO replace this with an appropriate error messages
			case ListRemoveError { ErrorType: ListRemoveError.Types.OtherFailure }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAutoButtonError.ButtonNotFoundCaption,ErrorData.RemoveAutoButtonError.ButtonNotFoundMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}

	}

	private void MoveButtonUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveButtonDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}
	


	private void AddInputButton_Click(object sender, RoutedEventArgs e) {
		
		Inputs.Add(DefaultEditingDataValues.DefaultInputEditingData);
	}

	private void RemoveInputButton_Click(object sender, RoutedEventArgs e) {
		
		if (SelectedInput is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<ListRemoveError> result = Inputs.Remove(SelectedInput);

		switch (result.Resolve()) {
			
			case Success:
				return;

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

	private void MoveInputUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveInputDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}