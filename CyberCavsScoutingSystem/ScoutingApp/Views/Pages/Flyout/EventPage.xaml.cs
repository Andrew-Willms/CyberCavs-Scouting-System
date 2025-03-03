using System.Collections.Generic;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class EventPage : ContentPage {

	public static string Route => "Event";

	public IAppManager AppManager { get; }

	public ReadOnlyList<string> Events { get; } = new List<string> {
		"Test Event",
		"Waterloo",
		"Windsor",
		"DCMP"
	}.ToReadOnly();

	public EventPage(IAppManager appManager) {

		AppManager = appManager;

		BindingContext = this;
		InitializeComponent(); 
	}

}