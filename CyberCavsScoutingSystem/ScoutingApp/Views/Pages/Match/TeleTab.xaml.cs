using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages.Match; 



public partial class TeleTab : ContentPage {

	public static string Route => "Tele";

	// These can't be static or PropertyChanged events on them won't work.
	private IAppManager AppManager => ServiceHelper.GetService<IAppManager>();

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.TeleTabInputs;

	public TeleTab() {

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		BindingContext = this;
		InitializeComponent();
	}

}