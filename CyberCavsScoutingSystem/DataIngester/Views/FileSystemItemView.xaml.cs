using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

namespace DataIngester.Views; 



public partial class FileSystemItemView : ContentView {

	public FileSystemItemView() {
		InitializeComponent();
	}

}




[INotifyPropertyChanged]
public partial class FileSystemItem {

	[ObservableProperty]
	private string _Path = "";

	[ObservableProperty]
	private bool _IsAccessible;

}