using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages.Flyout;
using ScoutingApp.Views.Pages.Match;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace ScoutingApp;



public static class MauiProgram {

	public static MauiApp CreateMauiApp() {

		MauiAppBuilder builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
			.ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureMauiHandlers(handlers => {
				handlers.AddHandler<CameraBarcodeReaderView, CameraBarcodeReaderViewHandler>();
				handlers.AddHandler<CameraView, CameraViewHandler>();
				handlers.AddHandler<BarcodeGeneratorView, BarcodeGeneratorViewHandler>();
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IAppManager, AppManager>();
		builder.Services.AddTransient<SetupTab>();
		builder.Services.AddTransient<AutoTab>();
		builder.Services.AddTransient<TeleTab>();
		builder.Services.AddTransient<EndgameTab>();
		builder.Services.AddTransient<ConfirmTab>();
		builder.Services.AddTransient<ScoutPage>();
		builder.Services.AddTransient<EventPage>();
		builder.Services.AddTransient<MatchDetailsPage>();
		builder.Services.AddTransient<SavedMatchesPage>();

		return builder.Build();
	}

}