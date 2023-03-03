using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages; 



public partial class AutoTab : ContentPage {

	public static string Route => "Auto";

	public ObservableCollection<DataField> Inputs { get; } = new(new() {

		new IntegerDataField(new() { Name = "High Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "High Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Mid Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "Mid Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Low Cones", InitialValue = 0, MinValue = 0, MaxValue = 9 }),
		new IntegerDataField(new() { Name = "Low Cubes", InitialValue = 0, MinValue = 0, MaxValue = 9 }),

		new SelectionDataField(new() { Name = "Charge Station", OptionNames = new List<string> {
			"None",
			"Attempted",
			"Docked",
			"Engaged"
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Mobility", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()})

	});

	public AutoTab() {

		BindingContext = this;
		InitializeComponent();
	}

}