using System.Windows;

namespace GameMakerWpf.Views;



/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

	public MainWindow() {

		DataContext = ApplicationManager.GameProject.EditingData;

		InitializeComponent();

	}

}