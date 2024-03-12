using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages.Match; 



public partial class TeleTab : ContentPage {

	public static string Route => "Tele";

	private IAppManager AppManager { get; }

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.TeleTabInputs;

	public TeleTab(IAppManager appManager) {

		AppManager = appManager;
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		BindingContext = this;
		InitializeComponent();
	}

}