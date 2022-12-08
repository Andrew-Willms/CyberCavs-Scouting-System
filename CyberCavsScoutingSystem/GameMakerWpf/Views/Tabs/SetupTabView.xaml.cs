using System;
using System.ComponentModel;
using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.Tabs;



public partial class SetupTabView : AppManagerDependent, INotifyPropertyChanged {

	
	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.Manager.GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<InputEditor, InputEditingData> Inputs => GameEditor.SetupTabInputs;

	private InputEditor? _SelectedInput;
	public InputEditor? SelectedInput {
		get => _SelectedInput;
		set {
			_SelectedInput = value;
			OnPropertyChanged(nameof(SelectedInput));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => App.Manager.SelectedDataField is not null;



	public SetupTabView() {

		DataContext = this;

		InitializeComponent();
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}


	
	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}