using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages; 



public partial class ConfirmTab : ContentPage {

	public static string Route => "Confirm";

	// These can't be static or PropertyChanged events on them won't work.
	private IAppManager AppManager => ServiceHelper.GetService<IAppManager>();

	public ConfirmTab() {

		//AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		InitializeComponent();
	}

}