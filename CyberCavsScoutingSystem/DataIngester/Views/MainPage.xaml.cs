using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace DataIngester.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	private FileViewModel _TargetFile = new() { };
	public FileViewModel TargetFile {
		get => _TargetFile;
		set {
			_TargetFile = value;
			OnPropertyChanged(nameof(TargetFile));
		}
	}

	public ObservableCollection<FileViewModel> SourceFiles { get; } = new() {
		new() { Path = "path 1" },
		new() { Path = "path 2" },
		new() { Path = "path 3" }
	};

	public ObservableCollection<string> LogMessages { get; } = new();

	public MainPage() {
		InitializeComponent();
		BindingContext = this;
	}


	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}