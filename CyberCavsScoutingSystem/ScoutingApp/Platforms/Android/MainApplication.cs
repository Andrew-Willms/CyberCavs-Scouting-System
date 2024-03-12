﻿using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ScoutingApp;



[Application]
public class MainApplication : MauiApplication {

	public MainApplication(nint handle, JniHandleOwnership ownership)
		: base(handle, ownership) {
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

}