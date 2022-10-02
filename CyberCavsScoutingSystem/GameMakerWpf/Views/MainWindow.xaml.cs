using System.Windows;

namespace GameMakerWpf.Views;



public partial class MainWindow : Window {

	public MainWindow() {

		DataContext = ApplicationManager.GameEditingData;

		InitializeComponent();
	}

}