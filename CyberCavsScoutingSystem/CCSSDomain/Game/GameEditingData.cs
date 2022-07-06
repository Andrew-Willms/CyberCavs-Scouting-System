using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using CCSSDomain.Alliance;
using WPFUtilities.Validation;
using WPFUtilities.Validation.Inputs;

namespace CCSSDomain.Game;



public class GameEditingData : INotifyPropertyChanged {

	private GameEditingData() {

		Year = new(GameValidator.YearConversionPair,
			DateTime.Now.Year.ToString(),
			new ValidationSet<int, ErrorSeverity>(GameValidator.YearValidator_YearNotNegative),
			new ValidationSet<int, ErrorSeverity>(GameValidator.YearValidator_YearNotFarFuture),
			new ValidationSet<int, ErrorSeverity>(GameValidator.YearValidator_YearNotPredateFirst)
		);

		Name = new(GameValidator.NameConverter, GameValidator.NameInverter,
			DateTime.Now.Year.ToString(),
			new ValidationSet<string, ErrorSeverity>(GameValidator.NameValidator_Length)
		);

		Description = new(GameValidator.DescriptionConverter, GameValidator.DescriptionInverter, "");

		Version = new(GameValidator.VersionConversionPair,
			new SingleInput<uint, string, ErrorSeverity>(GameValidator.VersionComponentNumberConversionPair, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameValidator.VersionComponentNumberConversionPair, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameValidator.VersionComponentNumberConversionPair, "0"),
			new SingleInput<string, string, ErrorSeverity>(GameValidator.VersionDescriptionConversionPair, "")
		);

		RobotsPerAlliance = new(GameValidator.RobotsPerAllianceConversionPair, "3");
		AlliancesPerMatch = new(GameValidator.AlliancesPerMatchConversionPair, "2");

		Alliances = new();
	}



	public static GameEditingData GetDefaultEditingData() {

		GameEditingData gameEditingData = new();

		gameEditingData.Alliances.Add(new(gameEditingData));
		gameEditingData.Alliances.Add(new(gameEditingData));

		gameEditingData.Alliances[0].Name.InputObject = "Red Alliance";
		gameEditingData.Alliances[1].Name.InputObject = "Blue Alliance";

		gameEditingData.Alliances[0].AllianceColor.OutputObject = Color.FromRgb(255, 0, 0);
		gameEditingData.Alliances[1].AllianceColor.OutputObject = Color.FromRgb(0, 0, 255);

		return gameEditingData;
	}



	public MultiInput<VersionNumber, ErrorSeverity, uint, uint, uint, string> Version { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }
	public SingleInput<int, string, ErrorSeverity> Year { get; }
	public SingleInput<string, string, ErrorSeverity> Description { get; }

	public SingleInput<int, string, ErrorSeverity> RobotsPerAlliance { get; }
	public SingleInput<int, string, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AllianceNameChanged { get; } = new();

	public ObservableCollection<AllianceEditingData> Alliances { get; } // = new();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}