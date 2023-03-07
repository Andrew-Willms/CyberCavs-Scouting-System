using System.ComponentModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages; 



public partial class AutoTab : ContentPage, INotifyPropertyChanged {

	public static string Route => "Auto";

	// These can't be static or PropertyChanged events on them won't work.
	private IAppManager AppManager => ServiceHelper.GetService<IAppManager>();

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.AutoTabInputs;


	public AutoTab() {

		BindingContext = this;
		InitializeComponent();
	}

}