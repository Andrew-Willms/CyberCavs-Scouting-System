﻿using System;
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
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.Tabs;



public partial class EndgameTabView : AppManagerDependent, INotifyPropertyChanged  {

	private static IErrorPresenter ErrorPresenter => App.ServiceProvider.GetRequiredService<IErrorPresenter>();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.ServiceProvider.GetRequiredService<IAppManager>().GameEditor;

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

	public bool RemoveButtonIsEnabled => App.ServiceProvider.GetRequiredService<IAppManager>().SelectedDataField is not null;



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

		IListRemoveResult<InputEditor> result = Inputs.Remove(SelectedInput);

		switch (result) {

			case IListRemoveResult<InputEditor>.Success:
				return;

			case IListRemoveResult<InputEditor>.ItemNotFound error:
				ErrorPresenter.DisplayError(error, RemoveFromListErrors.RemoveEndgameInputError);
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
