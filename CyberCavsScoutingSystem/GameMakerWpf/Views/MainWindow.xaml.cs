using System.Windows;

namespace GameMakerWpf.Views;



public partial class MainWindow : Window, IGameMakerMainView {

	public MainWindow() {

		DataContext = ApplicationManager.GameEditingData;

		InitializeComponent();
	}



	private void Save_Clicked(object sender, RoutedEventArgs e) {

		ApplicationManager.SaveGameProject();
	}

	private void SaveAs_Clicked(object sender, RoutedEventArgs e) {

		ApplicationManager.SaveGameProjectAs();
	}

	private void Open_Clicked(object sender, RoutedEventArgs e) {

		ApplicationManager.OpenGameProject();
	}

	private void New_Clicked(object sender, RoutedEventArgs e) {

		ApplicationManager.NewGameProject();
	}

}