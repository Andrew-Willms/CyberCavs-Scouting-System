using System;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplates;



public partial class MultiIntegerDataFieldInputView : ContentView {

	public MultiIntegerDataFieldInputView() {
		InitializeComponent();
	}

	private void IncrementButton_OnClick(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value++;
	}

	private void DecrementButton_OnClicked(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value--;
	}

}