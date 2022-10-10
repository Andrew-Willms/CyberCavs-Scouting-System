using System.Windows.Controls;
using CCSSDomain;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views;



public partial class MetaTabView : UserControl {

	private static GameEditingData GameEditingData => ApplicationManager.GameEditingData;

	public static SingleInput<string, string, ErrorSeverity> GameName => GameEditingData.Name;
	public static SingleInput<string, string, ErrorSeverity> Description => GameEditingData.Description;
	public static SingleInput<int, string, ErrorSeverity> Year => GameEditingData.Year;
	public static MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version => GameEditingData.Version;



	public MetaTabView() {

		DataContext = this;

		InitializeComponent();
	}

}