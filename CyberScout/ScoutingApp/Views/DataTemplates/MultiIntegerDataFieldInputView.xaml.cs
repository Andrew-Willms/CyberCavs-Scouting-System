using System;
using Domain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplates;



public partial class MultiIntegerDataFieldInputView : ContentView {

	public MultiIntegerDataFieldInputView() {
		InitializeComponent();
	}

	private void IncrementOneButton_OnClick(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value++;
	}

	private void IncrementFiveButton_OnClick(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value += 5;
	}

	private void IncrementTenButton_OnClick(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value += 10;
	}

	private void DecrementButton_OnClicked(object? sender, EventArgs e) {

		MultiIntegerInputDataCollector dataField = BindingContext as MultiIntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value--;
	}

}