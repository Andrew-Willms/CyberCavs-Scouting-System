using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Results;
using static ScoutingApp.IGameSpecRetrievalResult;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace ScoutingApp.AppManagement;



public interface IAppManager : INotifyPropertyChanged {

	public MatchDataCollector ActiveMatchData { get; }

	public Task ApplicationStartup();

	public Task StartNewMatch();

	public Task StoreMatchData();

	public Event OnMatchStarted { get; }

}



public class AppManager : IAppManager, INotifyPropertyChanged {

	private MatchDataCollector _ActiveMatchData = null!;
	public MatchDataCollector ActiveMatchData {
		get => _ActiveMatchData;
		private set {
			_ActiveMatchData = value;
			OnPropertyChanged(nameof(ActiveMatchData));
		}
	}

	public Event OnMatchStarted { get; } = new();

	private static IErrorPresenter ErrorPresenter => ServiceHelper.GetService<IErrorPresenter>();



	public async Task ApplicationStartup() {

		await StartNewMatch();
	}

	public async Task StartNewMatch() {

		IGameSpecRetrievalResult result = await App.GetGameSpec();

		while (result is Loading) {

			await Task.Delay(100);
			result = await App.GetGameSpec();
		}

		if (result is Error error) {

			ErrorPresenter.DisplayError("Error ", error.Message);
		}

		GameSpec gameSpec = (result as IResult<GameSpec>.Success)?.Value ?? throw new UnreachableException();

		ActiveMatchData = new(gameSpec);

		OnMatchStarted.Invoke();
	}

	public async Task StoreMatchData() {





		ActiveMatchData.ConvertDataToCsv();



		throw new System.NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}