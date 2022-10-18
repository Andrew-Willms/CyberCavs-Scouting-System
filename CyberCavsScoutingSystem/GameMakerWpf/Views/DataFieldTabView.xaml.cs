using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using GameMakerWpf.Domain;
using GameMakerWpf.DomainData;

namespace GameMakerWpf.Views;



public partial class DataFieldTabView : UserControl, INotifyPropertyChanged {

	private static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public static ReadOnlyObservableCollection<GeneralDataFieldEditingData> DataFields => GameEditingData.DataFields;

	private GeneralDataFieldEditingData? _SelectedDataField;
	public GeneralDataFieldEditingData? SelectedDataField {
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
	}




	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		GameEditingData.AddGeneratedDataField();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedDataField is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no DataField is selected.");
		}

		GameEditingData.RemoveDataField(SelectedDataField);
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