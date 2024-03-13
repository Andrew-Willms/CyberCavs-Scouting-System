﻿using System;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplates; 



public partial class IntegerDataFieldInputView : ContentView {

	public IntegerDataFieldInputView() {
		InitializeComponent();
	}

	private void IncrementButton_OnClick(object? sender, EventArgs e) {

		IntegerInputDataCollector dataField = BindingContext as IntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value++;
	}

	private void DecrementButton_OnClicked(object? sender, EventArgs e) {

		IntegerInputDataCollector dataField = BindingContext as IntegerInputDataCollector ?? throw new InvalidOperationException();
		dataField.Value--;
	}

}