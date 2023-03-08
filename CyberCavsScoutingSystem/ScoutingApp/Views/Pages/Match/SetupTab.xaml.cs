using System.Linq;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.Views.Pages.Match; 



public partial class SetupTab : ContentPage {

	public static string Route => "Setup";

	private IAppManager AppManager { get; }

	public uint? MatchNumber {

		//TODO this sometimes throws a null reference exception, figure out why (it's ActiveMatchData that is null)
		get => AppManager.ActiveMatchData.MatchNumber.HasValue
			? AppManager.ActiveMatchData.MatchNumber.Value
			: null;

		set => AppManager.ActiveMatchData.MatchNumber = value is not null
			? ((uint)value).Optionalize()
			: Optional.NoValue;
	}

	public uint? ReplayNumber {

		get => AppManager.ActiveMatchData.ReplayNumber.HasValue
			? AppManager.ActiveMatchData.ReplayNumber.Value
			: null;

		set => AppManager.ActiveMatchData.ReplayNumber = value is not null
			? ((uint)value).Optionalize()
			: Optional.NoValue;
	}

	public bool? IsPlayoff {

		get => AppManager.ActiveMatchData.IsPlayoff.HasValue
			? AppManager.ActiveMatchData.IsPlayoff.Value
			: null;

		set => AppManager.ActiveMatchData.IsPlayoff = value is not null
			? ((bool)value).Optionalize()
			: Optional.NoValue;
	}

	public uint? TeamNumber {

		get => AppManager.ActiveMatchData.TeamNumber.HasValue
			? AppManager.ActiveMatchData.TeamNumber.Value
			: null;

		set => AppManager.ActiveMatchData.TeamNumber = value is not null
			? ((uint)value).Optionalize()
			: Optional.NoValue;
	}

	public string? Alliance {
		get => AppManager.ActiveMatchData.Alliance.HasValue
			? AppManager.ActiveMatchData.Alliance.Value.Name
			: null;

		set => AppManager.ActiveMatchData.Alliance = Alliances.Contains(value) 
			? AppManager.ActiveMatchData.GameSpecification.Alliances.First(x => x.Name == value).Optionalize() 
			: Optional.NoValue;
	}

	public ReadOnlyList<string> Alliances => AppManager.ActiveMatchData.GameSpecification.Alliances.Select(x => x.Name).ToReadOnly();

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.SetupTabInputs;

	public SetupTab(IAppManager appManager) {

		AppManager = appManager;

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(MatchNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(ReplayNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(IsPlayoff)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(TeamNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliance)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliances)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		BindingContext = this;
		InitializeComponent();
	}

}