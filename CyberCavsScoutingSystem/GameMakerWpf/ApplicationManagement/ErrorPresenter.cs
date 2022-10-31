using System.Windows;

namespace GameMakerWpf.ApplicationManagement; 



public class ErrorPresenter : IErrorPresenter {
	
	public void DisplayError(string caption, string message) {

		MessageBox.Show(message, caption, MessageBoxButton.OK);
	}

}