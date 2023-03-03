using System.Collections.Generic;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages; 



public partial class EventPage : ContentPage {

	public static string Route => "Event";

	public ReadOnlyList<string> Events { get; set; } = new List<string> {
		"Waterloo",
		"Windsor",
		"Ontario Champs"
	}.ToReadOnly();

	public EventPage() {

		BindingContext = this;
		InitializeComponent();
	}

}