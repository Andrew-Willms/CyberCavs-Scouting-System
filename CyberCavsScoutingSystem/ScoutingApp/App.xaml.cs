﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp;



public partial class App : Application {

	public App() {

		InitializeComponent();
		ServiceHelper.GetService<IAppManager>().ApplicationStartup().GetAwaiter().GetResult();
	}

	protected override Window CreateWindow(IActivationState? activationState) {
		return new(new AppShell());
	}

}