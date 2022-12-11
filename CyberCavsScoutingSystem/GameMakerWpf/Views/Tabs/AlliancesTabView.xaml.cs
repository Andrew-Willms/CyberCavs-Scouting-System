using System;
using System.ComponentModel;
using System.Diagnostics;
using CCSSDomain;
using GameMakerWpf.AppManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.Validation.Inputs;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Views.Tabs;



public partial class AlliancesTabView : AppManagerDependent, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.Manager.GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public ObservableList<AllianceEditor, AllianceEditingData> Alliances => GameEditor.Alliances;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance => GameEditor.RobotsPerAlliance;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch => GameEditor.AlliancesPerMatch;

	private AllianceEditor? _SelectedAlliance;
	public AllianceEditor? SelectedAlliance {
		get => _SelectedAlliance;
		set {
			_SelectedAlliance = value;
			OnPropertyChanged(nameof(SelectedAlliance));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => _SelectedAlliance is not null;



	public AlliancesTabView() {

		DataContext = this;

		InitializeComponent();
	}



	private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		GameEditor.AddUniqueAlliance();
	}

	private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e) {

		if (SelectedAlliance is null) {
			throw new InvalidOperationException("The RemoveButton should not be enabled if no Alliance is selected.");
		}

		Result<ListRemoveError> result = Alliances.Remove(SelectedAlliance);

		switch (result.Resolve()) {

			case Success:
				return;

			case ListRemoveError { ErrorType: ListRemoveError.Types.ItemNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveAllianceError.AllianceNotFoundCaption, ErrorData.RemoveAllianceError.AllianceNotFoundMessage);
				return;

			//TODO replace with appropriate error message
			case ListRemoveError { ErrorType: ListRemoveError.Types.OtherFailure }:
				ErrorPresenter.DisplayError("todo", "todo");
				return;

			default:
				throw new UnreachableException();
		}
	}

	private void MoveUpButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, System.Windows.RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}