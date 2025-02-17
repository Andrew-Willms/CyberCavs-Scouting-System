using System.Diagnostics.CodeAnalysis;
using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace GameMakerWpf;



public partial class App : Application {

	[field: AllowNull, MaybeNull]
	public static ServiceProvider ServiceProvider {
		get {

			if (field is not null) {
				return field;
			}

			ServiceCollection services = new();
			ConfigureServices(services);
			field = services.BuildServiceProvider();

			return field;
		}
	}



	private void ApplicationStartup(object sender, StartupEventArgs e) {

		ServiceProvider.GetRequiredService<IAppManager>().ApplicationStartup();
	}

	private static void ConfigureServices(in IServiceCollection services) {

		services.AddTransient<IErrorPresenter, ErrorPresenter>();
		services.AddTransient<ISavePrompter, SavePrompter>();
		services.AddTransient<IPublisher, FilePublisher>();

		services.AddSingleton<ISaver, Saver>();
		services.AddSingleton<IMainView, MainWindow>();
		services.AddSingleton<IAppManager, AppManager>();
	}

}