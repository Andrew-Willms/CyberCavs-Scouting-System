using System.Windows;

namespace GameMakerWpf.AppManagement;



public interface IErrorPresenter {

	public void DisplayError(string caption, string message);

}



public class ErrorPresenter : IErrorPresenter {

	public void DisplayError(string caption, string message) {

		MessageBox.Show(message, caption, MessageBoxButton.OK);
	}

}