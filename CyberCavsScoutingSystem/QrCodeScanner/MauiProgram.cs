using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using ZXing.Net.Maui;
using CameraBarcodeReaderView = ZXing.Net.Maui.Controls.CameraBarcodeReaderView;
using CameraView = ZXing.Net.Maui.Controls.CameraView;
using BarcodeGeneratorView = ZXing.Net.Maui.Controls.BarcodeGeneratorView;

namespace QrCodeScanner;



public static class MauiProgram {

	public static MauiApp CreateMauiApp() {

		MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
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

		return builder.Build();
	}

}