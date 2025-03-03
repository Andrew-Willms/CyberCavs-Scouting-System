using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class ScoutPage : ContentPage {

	public static string Route => "Scout";

	public IAppManager AppManager { get; }

	//private bool SettingScout { get; set; }
	//private Lock SettingScoutLock { get; } = new();
	//private string? PendingValue { get; set; }
	//private Lock PendingValueLock { get; } = new();

	public ScoutPage(IAppManager appManager) {

		AppManager = appManager;

		BindingContext = this;
		InitializeComponent();
	}

	// ReSharper disable once AsyncVoidMethod required for UI handler
	//private async void ScoutName_OnTextChanged(object? sender, TextChangedEventArgs e) {
	//
	//	Entry entry = sender as Entry ?? throw new UnreachableException();
	//	string valueToSet = entry.Text;
	//
	//	lock (SettingScoutLock) {
	//		lock (PendingValueLock) {
	//			if (SettingScout) {
	//				PendingValue = valueToSet;
	//				return;
	//			}
	//
	//			PendingValue = valueToSet;
	//			SettingScout = true;
	//		}
	//	}
	//
	//	while (true) {
	//		await AppManager.SetLastScout(valueToSet);
	//
	//		lock (PendingValueLock) {
	//			if (valueToSet == PendingValue) {
	//				break;
	//			}
	//		}
	//	}
	//
	//	lock (SettingScoutLock) {
	//		SettingScout = false;
	//	}
	//
	//}

	//private void ScoutPage_OnNavigatedTo(object? sender, NavigatedToEventArgs e) {
	//	throw new System.NotImplementedException();
	//}

} 