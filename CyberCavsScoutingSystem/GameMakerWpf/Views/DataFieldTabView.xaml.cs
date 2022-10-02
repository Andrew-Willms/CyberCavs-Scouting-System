using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using GameMakerWpf.Domain;

namespace GameMakerWpf.Views;



/// <summary>
/// Interaction logic for DataFieldTabView.xaml
/// </summary>
public partial class DataFieldTabView : UserControl, INotifyPropertyChanged {

	public static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public ObservableCollection<AllianceEditingData> DataFields => ApplicationManager.GameEditingData.Alliances;

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
		throw new NotImplementedException();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
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
