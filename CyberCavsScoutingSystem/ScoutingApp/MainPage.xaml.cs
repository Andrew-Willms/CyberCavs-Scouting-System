﻿using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace ScoutingApp;



public partial class MainPage : ContentPage {

	int count = 0;

	public MainPage() {
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e) {

		count++;

		CounterBtn.Text = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

}