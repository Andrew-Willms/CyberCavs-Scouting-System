using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

namespace DataIngester.Views; 



public partial class FileView : ContentView {

	private FileViewModel FileViewModel { get; set; } = null!;

	public FileView() {
		InitializeComponent();
	}

	private void OnBindingContextChanged(object? sender, EventArgs e) {

		FileViewModel = BindingContext as FileViewModel ?? throw new ArgumentException();
	}

	private void Button_OnClicked(object? sender, EventArgs e) {

		FileViewModel.IsBeingEdited = !FileViewModel.IsBeingEdited;
	}

}




[INotifyPropertyChanged]
public partial class FileViewModel {

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotBeingEdited))]
	[NotifyPropertyChangedFor(nameof(ButtonText))]
	private bool _IsBeingEdited;

	public bool IsNotBeingEdited => !_IsBeingEdited;

	public string ButtonText => IsBeingEdited ? "Done" : "Edit";

	[ObservableProperty]
	private string _Path = "";

}