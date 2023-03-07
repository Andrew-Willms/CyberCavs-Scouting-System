using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class ScoutPage : ContentPage, INotifyPropertyChanged {

	public static string Route => "Scout";

	private string _ScoutName = "";
	public string ScoutName {
		get => _ScoutName;
		set {
			_ScoutName = value;
			OnPropertyChanged(nameof(ScoutName));
		}
	}

	public ScoutPage() {
		InitializeComponent();
	}

	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

} 