using System;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using IntegerDataField = CCSSDomain.DataCollectors.IntegerDataField;

namespace ScoutingApp.Views.DataTemplates; 



public partial class IntegerDataFieldInputView : ContentView {

	public IntegerDataFieldInputView() {
		InitializeComponent();
	}

	private void IncrementButton_OnClick(object? sender, EventArgs e) {

		IntegerDataField dataField = BindingContext as IntegerDataField ?? throw new InvalidOperationException();
		dataField.Value++;
	}

	private void DecrementButton_OnClicked(object? sender, EventArgs e) {

		IntegerDataField dataField = BindingContext as IntegerDataField ?? throw new InvalidOperationException();
		dataField.Value--;
	}

}