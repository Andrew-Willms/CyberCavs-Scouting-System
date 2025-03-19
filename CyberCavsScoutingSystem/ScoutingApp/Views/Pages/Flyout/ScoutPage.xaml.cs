using System;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout;



public partial class ScoutPage : ContentPage {

	public static string Route => "Scout";

	private IAppManager AppManager { get; }
	private IErrorPresenter ErrorPresenter { get; }

	public string ScoutName {
		get => AppManager.Scout;
		set {
			AppManager.Scout = value;
			OnPropertyChanged();
		}
	}

	public string? Error {
		get;
		set {
			field = value;
			OnPropertyChanged();
		}
	}



	public ScoutPage(IAppManager appManager, IErrorPresenter errorPresenter) {

		AppManager = appManager;
		ErrorPresenter = errorPresenter;

		Task.Run(async () => {
			string? name = await AppManager.GetScoutName();

			MainThread.BeginInvokeOnMainThread(() => {
				Error = name is null
					? "Could not load the last scout from the data store."
					: null;

				ScoutName = name ?? string.Empty;
			});
		});

		BindingContext = this;
		InitializeComponent();
	}

	private void SaveButton_Clicked(object? sender, EventArgs e) {

		Task.Run(async () => {

			bool success = await AppManager.SetScoutName(ScoutName);

			MainThread.BeginInvokeOnMainThread(() => {
				Error = success ? null : "There was an error saving the scout to the data store.";
			});
		});

		OnPropertyChanged();
	}

} 