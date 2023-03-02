using System.Collections.Generic;
using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Tabs; 



public partial class EndgameTab : ContentPage {

	public ObservableCollection<DataField> Inputs { get; } = new(new() {

		new SelectionDataField(new() { Name = "Charge Station", OptionNames = new List<string> {
			"None",
			"Attempted",
			"Docked",
			"Engaged"
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Shelf Cones", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Chute Cones", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Upright Cones", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Tipped Cones", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Shelf Cubes", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Chute Cubes", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new SelectionDataField(new() { Name = "Ground Cubes", OptionNames = new List<string> {
			"Yes",
			"No",
		}.ToReadOnly()}),

		new TextDataField(new() { Name = "Comments"}),

	});

	public EndgameTab() {

		BindingContext = this;
		InitializeComponent();
	}

}