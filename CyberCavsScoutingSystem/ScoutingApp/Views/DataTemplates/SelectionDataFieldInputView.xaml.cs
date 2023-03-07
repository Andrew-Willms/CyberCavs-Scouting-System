using System;
using System.Diagnostics;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplates; 



public partial class SelectionDataFieldInputView : ContentView {

	public SelectionDataFieldInputView() {
		InitializeComponent();
	}

	private void Picker_OnSelectedIndexChanged(object? sender, EventArgs e) {


		SelectionInputDataCollector collector = BindingContext as SelectionInputDataCollector ?? throw new();

		Debug.WriteLine($"{collector.SelectedOption}");

		//bool test = true;
	}
}