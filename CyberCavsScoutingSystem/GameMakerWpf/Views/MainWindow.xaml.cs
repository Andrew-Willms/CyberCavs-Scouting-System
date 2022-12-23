using System.Windows;
using GameMakerWpf.AppManagement;
using Microsoft.Extensions.DependencyInjection;

namespace GameMakerWpf.Views;



public partial class MainWindow : Window, IGameMakerMainView {

	public MainWindow() {

		DataContext = App.ServiceProvider.GetRequiredService<IAppManager>().GameEditor;

		InitializeComponent();
	}



	private void Save_Execute(object sender, RoutedEventArgs e) {

		App.ServiceProvider.GetRequiredService<IAppManager>().SaveGameProject();
	}

	private void SaveAs_Execute(object sender, RoutedEventArgs e) {

		App.ServiceProvider.GetRequiredService<IAppManager>().SaveGameProjectAs();
	}

	private void Open_Execute(object sender, RoutedEventArgs e) {

		App.ServiceProvider.GetRequiredService<IAppManager>().OpenGameProject();
	}

	private void New_Execute(object sender, RoutedEventArgs e) {

		App.ServiceProvider.GetRequiredService<IAppManager>().NewGameProject();
	}

	private void Publish_Execute(object sender, RoutedEventArgs e) {

		App.ServiceProvider.GetRequiredService<IAppManager>().Publish();
	}

}