using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Tabs; 



public partial class TeleTab : ContentPage {

	public ObservableCollection<DataField> Inputs { get; } = new(new() {

		new IntegerDataField(new() { Name = "High Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "High Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Mid Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "Mid Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Low Cones", InitialValue = 0, MinValue = 0, MaxValue = 9 }),
		new IntegerDataField(new() { Name = "Low Cubes", InitialValue = 0, MinValue = 0, MaxValue = 9 }),

		new TextDataField(new() { Name = "Comments"}),

		new SelectionDataField(new() { Name = "Climb Level", OptionNames = new List<string> {
			"None",
			"Attempted",
			"Docked",
			"Engaged"
		}.ToReadOnly()})

	});

	public TeleTab() {

		BindingContext = this;
		InitializeComponent();
	}

}