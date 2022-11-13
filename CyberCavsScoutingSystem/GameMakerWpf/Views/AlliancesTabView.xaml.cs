﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CCSSDomain;
using UtilitiesLibrary.Validation.Inputs;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;

namespace GameMakerWpf.Views;



public partial class AlliancesTabView : UserControl, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => ApplicationManager.GameEditor;
	public ReadOnlyObservableCollection<AllianceEditor> Alliances => GameEditor.Alliances;
	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance => GameEditor.RobotsPerAlliance;
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch => GameEditor.AlliancesPerMatch;

	private AllianceEditor? _SelectedAlliance;
	public AllianceEditor? SelectedAlliance {
		get => _SelectedAlliance;
		set {
			_SelectedAlliance = value;
			OnPropertyChanged(nameof(SelectedAlliance));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => _SelectedAlliance is not null;



	public AlliancesTabView() {

		DataContext = this;

		InitializeComponent();

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);
	}



	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		GameEditor.AddUniqueAlliance();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedAlliance is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<GameEditor.RemoveAllianceError> result = GameEditor.RemoveAlliance(SelectedAlliance);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case GameEditor.RemoveAllianceError { ErrorType: GameEditor.RemoveAllianceError.Types.AllianceNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAllianceError.AllianceNotFoundCaption, ErrorData.RemoveAllianceError.AllianceNotFoundMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	private void MoveUpButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, System.Windows.RoutedEventArgs e) {
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
