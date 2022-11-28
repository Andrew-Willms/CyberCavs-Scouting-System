using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;

namespace GameMakerWpf.Views.Tabs;



public partial class SetupTabView : UserControl, INotifyPropertyChanged {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => ApplicationManager.GameEditor;
	public ReadOnlyObservableCollection<InputEditor> Inputs => GameEditor.SetupTabInputs;

	//private DataFieldEditor? _SelectedInput;
	public DataFieldEditor? SelectedInput {
		get => ApplicationManager.SelectedDataField;
		set {
			ApplicationManager.SelectedDataField = value;
			OnPropertyChanged(nameof(SelectedInput));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => ApplicationManager.SelectedDataField is not null;



	public SetupTabView() {

		DataContext = this;

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);

		InitializeComponent();
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {
		throw new System.NotImplementedException();
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {
		throw new System.NotImplementedException();
	}

	private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
		throw new System.NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
		throw new System.NotImplementedException();
	}


	
	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}