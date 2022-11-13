using System.ComponentModel;
using System.Windows.Controls;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



public partial class MetaTabView : UserControl, INotifyPropertyChanged {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => ApplicationManager.GameEditor;
	public SingleInput<string, string, ErrorSeverity> GameName => GameEditor.Name;
	public SingleInput<string, string, ErrorSeverity> Description => GameEditor.Description;
	public SingleInput<int, string, ErrorSeverity> Year => GameEditor.Year;

	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version => GameEditor.Version;
	public SingleInput<uint, string, ErrorSeverity> VersionMajorNumber => GameEditor.VersionMajorNumber;
	public SingleInput<uint, string, ErrorSeverity> VersionMinorNumber => GameEditor.VersionMinorNumber;
	public SingleInput<uint, string, ErrorSeverity> VersionPatchNumber => GameEditor.VersionPatchNumber;
	public SingleInput<string, string, ErrorSeverity> VersionDescription => GameEditor.VersionDescription;


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