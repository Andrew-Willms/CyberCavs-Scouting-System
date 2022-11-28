using System.Windows;

namespace GameMakerWpf.AppManagement; 



public class ErrorPresenter : IErrorPresenter {
	
	public void DisplayError(string caption, string message) {

		MessageBox.Show(message, caption, MessageBoxButton.OK);
	}

}