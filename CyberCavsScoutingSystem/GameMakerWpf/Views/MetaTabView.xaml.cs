using System.ComponentModel;
using System.Windows.Controls;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



public partial class MetaTabView : UserControl, INotifyPropertyChanged {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditingData GameEditingData => ApplicationManager.GameEditingData;
	public SingleInput<string, string, ErrorSeverity> GameName => GameEditingData.Name;
	public SingleInput<string, string, ErrorSeverity> Description => GameEditingData.Description;
	public SingleInput<int, string, ErrorSeverity> Year => GameEditingData.Year;
	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version => GameEditingData.Version;



	public MetaTabView() {

		DataContext = this;

		InitializeComponent();

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}