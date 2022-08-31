using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using CCSSDomain;
using GameMakerWpf.Data;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Version = CCSSDomain.Version;

namespace GameMakerWpf.EditingData;



public class GameEditingData : INotifyPropertyChanged {

	private GameEditingData() {

		Year = new(GameNumbersValidator.YearConverter, GameNumbersValidator.YearInverter, DateTime.Now.Year.ToString(),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotNegative),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotFarFuture),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotPredateFirst)
		);

		Name = new(GameTextValidator.NameConverter, GameTextValidator.NameInverter, GameNameGenerator.GetRandomGameName(),
			new ValidationSet<string, ErrorSeverity>(GameTextValidator.NameValidator_Length)
		);

		Description = new(GameTextValidator.DescriptionConverter, GameTextValidator.DescriptionInverter, "");

		Version = new(GameVersionValidator.Converter, GameVersionValidator.Inverter,
			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter, GameVersionValidator.ComponentNumberInverter, "1"),
			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter, GameVersionValidator.ComponentNumberInverter, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter, GameVersionValidator.ComponentNumberInverter, "0"),
			new SingleInput<string, string, ErrorSeverity>(GameVersionValidator.DescriptionConverter, GameVersionValidator.DescriptionInverter, "")
		);

		RobotsPerAlliance = new(GameNumbersValidator.RobotsPerAllianceConverter, GameNumbersValidator.RobotsPerAllianceInverter, "3");
		AlliancesPerMatch = new(GameNumbersValidator.AlliancesPerMatchConverter, GameNumbersValidator.AlliancesPerMatchInverter, "2");

		Alliances = new();
	}



	public static GameEditingData GetDefaultEditingData() {

		GameEditingData gameEditingData = new();

		gameEditingData.Alliances.Add(new(gameEditingData, "Red Alliance", Color.FromRgb(255, 0, 0)));
		gameEditingData.Alliances.Add(new(gameEditingData, "Blue Alliance", Color.FromRgb(0, 0, 255)));

		return gameEditingData;
	}



	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }
	public SingleInput<int, string, ErrorSeverity> Year { get; }
	public SingleInput<string, string, ErrorSeverity> Description { get; }

	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance { get; }
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AllianceNameChanged { get; } = new();

	public ObservableCollection<AllianceEditingData> Alliances { get; } // = new();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}