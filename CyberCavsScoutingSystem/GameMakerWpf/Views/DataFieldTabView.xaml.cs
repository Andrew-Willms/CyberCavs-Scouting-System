using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.Editors;

namespace GameMakerWpf.Views;



public partial class DataFieldTabView : UserControl, INotifyPropertyChanged {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => ApplicationManager.GameEditor;
	public ReadOnlyObservableCollection<DataFieldEditor> DataFields => GameEditor.DataFields;

	private DataFieldEditor? _SelectedDataField;
	public DataFieldEditor? SelectedDataField {
		get => _SelectedDataField;
		set {
			_SelectedDataField = value;
			OnPropertyChanged(nameof(SelectedDataField));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => SelectedDataField is not null;



	public DataFieldTabView() {
		
		DataContext = this;

		InitializeComponent();

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);
	}



	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		GameEditor.AddGeneratedDataField();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedDataField is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no DataField is selected.");
		}

		GameEditor.RemoveDataField(SelectedDataField);
	}

	private void MoveUpButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string? propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}