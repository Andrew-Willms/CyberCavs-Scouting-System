using System.Windows;
using GameMakerWpf.AppManagement;

namespace GameMakerWpf.Views;



public partial class MainWindow : Window, IGameMakerMainView {

	public MainWindow() {

		DataContext = App.Manager.GameEditor;

		InitializeComponent();
	}



	private void Save_Execute(object sender, RoutedEventArgs e) {

		App.Manager.SaveGameProject();
	}

	private void SaveAs_Execute(object sender, RoutedEventArgs e) {

		App.Manager.SaveGameProjectAs();
	}

	private void Open_Execute(object sender, RoutedEventArgs e) {

		App.Manager.OpenGameProject();
	}

	private void New_Execute(object sender, RoutedEventArgs e) {

		App.Manager.NewGameProject();
	}

	private void Publish_Execute(object sender, RoutedEventArgs e) {

		App.Manager.Publish();
	}

}