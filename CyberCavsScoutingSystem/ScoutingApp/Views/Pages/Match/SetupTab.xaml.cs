using System;
using System.Linq;
using CCSSDomain.Data;
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

	public MatchType[] MatchTypes { get; } = Enum.GetValues<MatchType>();

	public MatchType? MatchType {

		get => AppManager.ActiveMatchData.MatchType.HasValue
			? AppManager.ActiveMatchData.MatchType.Value
			: null;

		set => AppManager.ActiveMatchData.MatchType = value is not null
			? ((MatchType) value).Optionalize()
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

	public uint? Alliance {
		get => AppManager.ActiveMatchData.Alliance.HasValue
			? AppManager.ActiveMatchData.Alliance.Value
			: null;

		set => AppManager.ActiveMatchData.Alliance = value is not null && Alliances.Count >= value
			? ((uint)value).Optionalize() 
			: Optional.NoValue;
	}

	public ReadOnlyList<string> Alliances => AppManager.ActiveMatchData.GameSpecification.Alliances.Select(x => x.Name).ToReadOnly();

	public ReadOnlyList<InputDataCollector> Inputs => AppManager.ActiveMatchData.SetupTabInputs;

	public SetupTab(IAppManager appManager) {

		AppManager = appManager;

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(MatchNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(ReplayNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(MatchType)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(TeamNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliance)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliances)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Inputs)));

		MatchType = CCSSDomain.Data.MatchType.Qualification;

		BindingContext = this;
		InitializeComponent();
	}

}