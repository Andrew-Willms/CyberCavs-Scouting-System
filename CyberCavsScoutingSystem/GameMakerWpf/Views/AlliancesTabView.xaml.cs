using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CCSSDomain;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



public partial class AlliancesTabView : UserControl, INotifyPropertyChanged {

	private static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public static ReadOnlyObservableCollection<AllianceEditingData> Alliances => GameEditingData.Alliances;

	public static SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance => GameEditingData.RobotsPerAlliance;

	public static SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch => GameEditingData.AlliancesPerMatch;

	private int _SelectedAllianceIndex = -1;
	public int SelectedAllianceIndex {
		get => _SelectedAllianceIndex;
		set {
			_SelectedAllianceIndex = value;
			OnPropertyChanged(nameof(SelectedAllianceIndex));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => SelectedAllianceIndex != -1;



	public AlliancesTabView() {

		DataContext = this;

		InitializeComponent();
	}



	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.AddAlliance();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.RemoveAlliance(GameEditingData.Alliances[SelectedAllianceIndex]);
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
