using System;
using System.ComponentModel;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.Views.Pages; 



public partial class SetupTab : ContentPage, INotifyPropertyChanged {

	public static string Route => "Setup";

	// These can't be static or PropertyChanged events on them won't work.
	private IAppManager AppManager { get; }

	//public uint? MatchNumber {

	//	get => AppManager.ActiveMatchData.MatchNumber.HasValue
	//		? AppManager.ActiveMatchData.MatchNumber.Value
	//		: null;

	//	set => AppManager.ActiveMatchData.MatchNumber = value is not null
	//		? ((uint)value).Optionalize()
	//		: Optional.NoValue;
	//}

	//public uint? ReplayNumber {

	//	get => AppManager.ActiveMatchData.ReplayNumber.HasValue
	//		? AppManager.ActiveMatchData.ReplayNumber.Value
	//		: null;

	//	set => AppManager.ActiveMatchData.ReplayNumber = value is not null
	//		? ((uint)value).Optionalize()
	//		: Optional.NoValue;
	//}

	public uint? MatchNumber {
		get {
			try {
				return AppManager.ActiveMatchData.MatchNumber.HasValue
					? AppManager.ActiveMatchData.MatchNumber.Value
					: null;

			} catch (Exception exception) {
				bool test = true;
			}

			return 3;
		}

		set {

			try {
				AppManager.ActiveMatchData.MatchNumber = value is not null
					? ((uint)value).Optionalize()
					: Optional.NoValue;

			} catch (Exception exception) {
				bool test = true;
			}

			OnPropertyChanged(nameof(MatchNumber));
		}
	}

	public uint? ReplayNumber {
		get {

			try {
				return AppManager.ActiveMatchData.ReplayNumber.HasValue
					? AppManager.ActiveMatchData.ReplayNumber.Value
					: null;

			} catch (Exception exception) {
				bool test = true;
			}

			return 3;
		}

		set {

			try {
				AppManager.ActiveMatchData.ReplayNumber = value is not null
					? ((uint)value).Optionalize()
					: Optional.NoValue;

			} catch (Exception exception) {
				bool test = true;
			}

			OnPropertyChanged(nameof(ReplayNumber));
		}
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

	public Alliance? Alliance {

		get => AppManager.ActiveMatchData.Alliance.HasValue
			? AppManager.ActiveMatchData.Alliance.Value
			: null;

		set => AppManager.ActiveMatchData.Alliance = value is not null
			? value.Optionalize()
			: Optional.NoValue;
	}

	public ReadOnlyList<Alliance> Alliances => AppManager.ActiveMatchData.GameSpecification.Alliances;

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

	private void SetupTab_OnLoaded(object? sender, EventArgs e) {

		OnPropertyChanged(nameof(MatchNumber));
		OnPropertyChanged(nameof(ReplayNumber));
		OnPropertyChanged(nameof(IsPlayoff));
		OnPropertyChanged(nameof(TeamNumber));
		OnPropertyChanged(nameof(Alliance));
		OnPropertyChanged(nameof(Alliances));
		OnPropertyChanged(nameof(Inputs));
	}


	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}