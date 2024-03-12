using System.ComponentModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages.Match; 



public partial class AutoTab : ContentPage, INotifyPropertyChanged {

	public static string Route => "Auto";

	private IAppManager AppManager { get; }

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.AutoTabInputs;


	public AutoTab(IAppManager appManager) {

		AppManager = appManager;
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		BindingContext = this;
		InitializeComponent();
	}

}