using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using GameMakerWpf.Domain;

namespace GameMakerWpf.Views;



public partial class DataFieldTabView : UserControl, INotifyPropertyChanged {

	private static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public static ReadOnlyObservableCollection<DataFieldEditingData> DataFields => GameEditingData.DataFields;

	private int _SelectedDataFieldIndex = -1;
	public int SelectedDataFieldIndex {
		get => _SelectedDataFieldIndex;
		set {
			_SelectedDataFieldIndex = value;
			OnPropertyChanged(nameof(SelectedDataFieldIndex));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => SelectedDataFieldIndex != -1;



	public DataFieldTabView() {
		
		DataContext = this;

		InitializeComponent();
	}




	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.AddDataField();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		ApplicationManager.RemoveDataField(DataFields[SelectedDataFieldIndex]);
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

}