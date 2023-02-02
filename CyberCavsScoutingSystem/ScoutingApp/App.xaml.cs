using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp;



public partial class App : Application {

	private static ServiceProvider? _ServiceProvider;
	public static ServiceProvider ServiceProvider {
		get {

			if (_ServiceProvider is not null) {
				return _ServiceProvider;
			}

			ServiceCollection services = new();
			ConfigureServices(services);
			_ServiceProvider = services.BuildServiceProvider();

			return _ServiceProvider;
		}
	}

	public App() {

		InitializeComponent();

		ServiceProvider.GetRequiredService<IAppManager>().ApplicationStartup();

		MainPage = ServiceProvider.GetRequiredService<IMainView>().AsPage();
	}

	private static void ConfigureServices(in IServiceCollection services) {

		services.AddSingleton<IAppManager, AppManager>();
		services.AddSingleton<IMainView, AppShell>();
	}

}