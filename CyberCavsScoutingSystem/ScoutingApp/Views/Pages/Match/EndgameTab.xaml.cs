using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages.Match; 



public partial class EndgameTab : ContentPage {

	public static string Route => "Endgame";

	private IAppManager AppManager { get; }

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.EndgameTabInputs;

	public EndgameTab(IAppManager appManager) {

		AppManager = appManager;
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		BindingContext = this;
		InitializeComponent();
	}

}