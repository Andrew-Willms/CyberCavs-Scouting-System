﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Domain;
using GameMakerWpf.DomainData;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



public partial class AlliancesTabView : UserControl, INotifyPropertyChanged {

	private static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public static ReadOnlyObservableCollection<AllianceEditingData> Alliances => GameEditingData.Alliances;

	public static SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance => GameEditingData.RobotsPerAlliance;

	public static SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch => GameEditingData.AlliancesPerMatch;

	private AllianceEditingData? _SelectedAlliance;
	public AllianceEditingData? SelectedAlliance {
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
	}



	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		GameEditingData.AddGeneratedAlliance();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedAlliance is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		GameEditingData.RemoveAlliance(SelectedAlliance);
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

}
