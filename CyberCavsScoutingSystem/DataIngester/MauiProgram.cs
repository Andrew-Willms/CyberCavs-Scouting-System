using System;
using DataIngester.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace DataIngester;



public static class MauiProgram {

	public static MauiApp CreateMauiApp() {

		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IMtpDeviceService, MtpDeviceService>();

		MauiApp app = builder.Build();

		app.UseStaticServiceResolver();

		return app;
	}

}



public static class StaticServiceResolver {

	private static IServiceProvider? _ServiceProvider;
	public static IServiceProvider ServiceProvider => 
		_ServiceProvider 
		?? throw new($"Service provider has not been initialized. Call {nameof(UseStaticServiceResolver)} before attempting to resolve a service.");

	private static void RegisterServiceProvider(IServiceProvider serviceProvider) {
		_ServiceProvider = serviceProvider;
	}

	public static T Resolve<T>() where T : class => ServiceProvider.GetRequiredService<T>();

	public static void UseStaticServiceResolver(this MauiApp app) {
		RegisterServiceProvider(app.Services);
	}

}