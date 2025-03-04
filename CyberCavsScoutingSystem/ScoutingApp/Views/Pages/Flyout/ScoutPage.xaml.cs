using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class ScoutPage : ContentPage {

	public static string Route => "Scout";

	public IAppManager AppManager { get; }

	public ScoutPage(IAppManager appManager) {

		AppManager = appManager;

		BindingContext = this;
		InitializeComponent();
	}

} 