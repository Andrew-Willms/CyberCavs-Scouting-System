using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace GameMakerWpf;



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



	private void ApplicationStartup(object sender, StartupEventArgs e) {

		ServiceProvider.GetRequiredService<IAppManager>().ApplicationStartup();
	}

	private static void ConfigureServices(in IServiceCollection services) {

		services.AddTransient<IErrorPresenter, ErrorPresenter>();
		services.AddTransient<ISavePrompter, SavePrompter>();
		services.AddTransient<IPublisher, Publisher>();

		services.AddSingleton<ISaver, Saver>();
		services.AddSingleton<IGameMakerMainView, MainWindow>();
		services.AddSingleton<IAppManager, AppManager>();
	}

}