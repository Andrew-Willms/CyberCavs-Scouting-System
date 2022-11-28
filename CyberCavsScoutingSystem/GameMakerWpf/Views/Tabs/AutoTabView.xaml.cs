﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;

namespace GameMakerWpf.Views.Tabs;



public partial class AutoTabView : UserControl, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => ApplicationManager.GameEditor;
	public ReadOnlyObservableCollection<ButtonEditor> Buttons => GameEditor.AutoButtons;

	private ButtonEditor? _SelectedButton;
	public ButtonEditor? SelectedButton {
		get => _SelectedButton;
		set {
			_SelectedButton = value;
			OnPropertyChanged(nameof(SelectedButton));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => _SelectedButton is not null;



	public AutoTabView() {

		DataContext = this;

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);

		InitializeComponent();
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {
		
		GameEditor.AddAutoButton(DefaultEditingDataValues.DefaultButtonEditingData);
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {
		
		if (SelectedButton is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<GameEditor.RemoveError> result = GameEditor.RemoveAutoButton(SelectedButton);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case GameEditor.RemoveError { ErrorType: GameEditor.RemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAutoButtonError.ButtonNotFoundCaption,ErrorData.RemoveAutoButtonError.ButtonNotFoundMessage);
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
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}