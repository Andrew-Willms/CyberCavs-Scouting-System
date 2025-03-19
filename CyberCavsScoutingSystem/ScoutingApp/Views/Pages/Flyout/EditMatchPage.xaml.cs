using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CCSSDomain.Data;
using CCSSDomain.DataCollectors;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
public partial class EditMatchPage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/Edit";
	public static string RouteFromSavedMatchesPage => "/Edit";

	public const string MatchDataNavigationParameterName = nameof(MatchDataDto);

	private IAppManager AppManager { get; }

	public MatchDataDto SavedMatch {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(SavedMatch));
		}
	} = null!;


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
			? ((MatchType)value).Optionalize()
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

	public ReadOnlyList<InputDataCollector> SetupInputs => AppManager.ActiveMatchData.SetupTabInputs;
	public ReadOnlyList<InputDataCollector> AutoInputs => AppManager.ActiveMatchData.AutoTabInputs;
	public ReadOnlyList<InputDataCollector> TeleInputs => AppManager.ActiveMatchData.TeleTabInputs;
	public ReadOnlyList<InputDataCollector> EndgameInputs => AppManager.ActiveMatchData.EndgameTabInputs;
	public ReadOnlyList<InputDataCollector> Inputs => SetupInputs.AppendRanges(AutoInputs, TeleInputs, EndgameInputs).ToReadOnly();

	private IErrorPresenter ErrorPresenter { get; }

	public ObservableCollection<string> Errors {
		get {

			ObservableCollection<string> errors = [];

			if (AppManager.Scout == "") {
				errors.Add("The scout name has not been set. Go to the Scout page in the flyout menu to set the scout name.");
			}

			if (AppManager.EventCode == "") {
				errors.Add("The event has not been set. Go to the Event page in the flyout menu to set the event.");
			}

			if (AppManager.ActiveMatchData.MatchNumber == Optional.NoValue) {
				errors.Add("The match number has not been set.");
			}

			if (AppManager.ActiveMatchData.ReplayNumber == Optional.NoValue) {
				errors.Add("The replay number has not been set.");
			}

			// todo figure out how to not have the default MatchType overridden
			//if (AppManager.ActiveMatchData.MatchType == Optional.NoValue) {
			//	errors.Add("MatchType has not been set.");
			//}

			if (AppManager.ActiveMatchData.Alliance == Optional.NoValue) {
				errors.Add("An alliance has not been set.");
			}

			if (AppManager.ActiveMatchData.TeamNumber == Optional.NoValue) {
				errors.Add("The team number has not been set.");
			}

			foreach (DataField dataField in AppManager.ActiveMatchData.DataFields) {
				foreach (string error in dataField.Errors) {
					errors.Add(error);
				}
			}

			return errors;
		}
	}

	public bool MatchIstValid => Errors.IsEmpty();



	public EditMatchPage(IAppManager appManager, IErrorPresenter errorPresenter) {

		AppManager = appManager;
		ErrorPresenter = errorPresenter;

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(MatchNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(ReplayNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(MatchType)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(TeamNumber)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliance)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(Alliances)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(SetupInputs)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(AutoInputs)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(TeleInputs)));
		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(nameof(EndgameInputs)));

		BindingContext = this;
		InitializeComponent();
	}



	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void SaveButton_OnClicked(object? sender, EventArgs e) {
		//await MatchDeleter(SavedMatch);
		await Shell.Current.GoToAsync("..");
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void CancelButton_OnClicked(object? sender, EventArgs e) {
		await Shell.Current.GoToAsync("..");
		//await Shell.Current.GoToAsync($"..{MatchQrCodePage.RouteFromSavedMatchesPage}"); // crashes
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}