using System.Windows;
using GameMakerWpf.ApplicationManagement;

namespace GameMakerWpf.Views;



public partial class MainWindow : Window, IGameMakerMainView {

	public MainWindow() {

		DataContext = ApplicationManager.GameEditor;

		InitializeComponent();
	}



	private void Save_Execute(object sender, RoutedEventArgs e) {

		ApplicationManager.SaveGameProject();
	}

	private void SaveAs_Execute(object sender, RoutedEventArgs e) {

		ApplicationManager.SaveGameProjectAs();
	}

	private void Open_Execute(object sender, RoutedEventArgs e) {

		ApplicationManager.OpenGameProject();
	}

	private void New_Execute(object sender, RoutedEventArgs e) {

		ApplicationManager.NewGameProject();
	}

}