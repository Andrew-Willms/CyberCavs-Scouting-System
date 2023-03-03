using System.ComponentModel;
using CCSSDomain.DataCollectors;
using MauiUtilities;
using Microsoft.Extensions.DependencyInjection;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.AppManagement;



public abstract class AppManagerDependent : DependentView<IAppManager> {

	protected override IAppManager SingletonGetter => App.ServiceProvider.GetRequiredService<IAppManager>();

}



public interface IAppManager : INotifyPropertyChanged {

	public Optional<MatchDataCollector> ActiveMatchData { get; }

	public void ApplicationStartup();

	public void StartNewMatch();

	public void StoreMatchData();

}



public class AppManager : IAppManager, INotifyPropertyChanged {

	private Optional<MatchDataCollector> _ActiveMatchData = Optional.NoValue;
	public Optional<MatchDataCollector> ActiveMatchData {
		get => _ActiveMatchData;
		private set {
			_ActiveMatchData = value;
			OnPropertyChanged(nameof(ActiveMatchData));
		}
	}



	private IMainView MainView => App.ServiceProvider.GetRequiredService<IMainView>();
	private static IErrorPresenter ErrorPresenter => App.ServiceProvider.GetRequiredService<IErrorPresenter>();


	public AppManager() {

	}





	public void ApplicationStartup() {
		throw new System.NotImplementedException();
	}

	public void StartNewMatch() {
		throw new System.NotImplementedException();
	}

	public void StoreMatchData() {
		throw new System.NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}