using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;

namespace GameMakerWpf.Views.Tabs;



public partial class DataFieldTabView : UserControl, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

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

		GameEditor.AddDataField(DefaultEditingDataValues.DefaultDataFieldEditingData);
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedDataField is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no DataField is selected.");
		}

		Result<GameEditor.RemoveError> result = GameEditor.RemoveDataField(SelectedDataField);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case GameEditor.RemoveError { ErrorType: GameEditor.RemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveDataFieldError.DataFieldNotFoundCaption, ErrorData.RemoveDataFieldError.DataFieldNotFoundMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
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