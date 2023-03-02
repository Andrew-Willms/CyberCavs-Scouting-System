using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Tabs; 



public partial class SetupTab : ContentPage {

	public ObservableCollection<DataField> Inputs { get; } = new(new() {

		new TextDataField(new() { Name = "Scout Name"}),

		new IntegerDataField(new() { Name = "Match Number", InitialValue = 0, MinValue = 0, MaxValue = 100 }),

		new IntegerDataField(new() { Name = "Team Number", InitialValue = 0, MinValue = 0, MaxValue = 10000 }),

		new SelectionDataField(new() { Name = "Alliance", OptionNames = new List<string> {
			"Red",
			"Blue"
		}.ToReadOnly()}),

	});

	public SetupTab() {

		BindingContext = this;
		InitializeComponent();
	}

}