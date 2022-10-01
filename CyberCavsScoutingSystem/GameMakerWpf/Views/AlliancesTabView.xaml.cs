using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CCSSDomain;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



/// <summary>
/// Interaction logic for AlliancesTabView.xaml
/// </summary>
public partial class AlliancesTabView : UserControl, INotifyPropertyChanged {

	public static GameEditingData GameEditingData => ApplicationManager.Game;

	public AlliancesTabView() {

		DataContext = this;

		InitializeComponent();
	}

	private int _SelectedAllianceIndex = -1;
	public int SelectedAllianceIndex {
		get => _SelectedAllianceIndex;
		set {
			_SelectedAllianceIndex = value;
			OnPropertyChanged(nameof(SelectedAllianceIndex));
		}
	}



	public ObservableCollection<AllianceEditingData> Alliances => ApplicationManager.Game.Alliances;

	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance => ApplicationManager.Game.RobotsPerAlliance;


	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.AddAlliance();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.RemoveAlliance(GameEditingData.Alliances[SelectedAllianceIndex]);
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string? propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}
