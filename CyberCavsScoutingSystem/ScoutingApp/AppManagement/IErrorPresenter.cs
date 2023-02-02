using Microsoft.Maui.Controls;

namespace ScoutingApp.AppManagement; 



public interface IErrorPresenter {

	public void DisplayError(string caption, string message);

}



public class ErrorPresenter : IErrorPresenter {

	public async void DisplayError(string caption, string message) {

		await Application.Current!.MainPage!.DisplayAlert("test", "test", "test");

	}

}