using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.DisplayData.Errors.ErrorData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using Microsoft.Extensions.DependencyInjection;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.Tabs;



public partial class TeleTabView : AppManagerDependent, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter => App.ServiceProvider.GetRequiredService<IErrorPresenter>();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.ServiceProvider.GetRequiredService<IAppManager>().GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<ButtonEditor, ButtonEditingData> Buttons => GameEditor.TeleButtons;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<InputEditor, InputEditingData> Inputs => GameEditor.TeleTabInputs;

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



	public TeleTabView() {

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

		IListRemoveResult<ButtonEditor> result = Buttons.Remove(SelectedButton);

		switch (result) {

			case Success:
				return;

			case IListRemoveResult<ButtonEditor>.ItemNotFound error:
				ErrorPresenter.DisplayError(error, RemoveFromListErrors.RemoveTeleButtonError);
				return;

			default:
				throw new UnreachableException();
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

		IListRemoveResult<InputEditor> result = Inputs.Remove(SelectedInput);

		switch (result) {

			case Success:
				return;

			case IListRemoveResult<InputEditor>.ItemNotFound error:
				ErrorPresenter.DisplayError(error, RemoveFromListErrors.RemoveTeleInputError);
				return;

			default:
				throw new UnreachableException();
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
