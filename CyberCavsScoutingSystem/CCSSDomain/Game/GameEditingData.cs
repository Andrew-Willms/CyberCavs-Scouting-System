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

		Year = new(GameEditingDataValidator.YearConverter, GameEditingDataValidator.YearInverter,
			DateTime.Now.Year.ToString(),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotNegative),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotFarFuture),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotPredateFirst)
		);

		Name = new(GameEditingDataValidator.NameConverter, GameEditingDataValidator.NameInverter,
			DateTime.Now.Year.ToString(),
			new ValidationSet<string, ErrorSeverity>(GameEditingDataValidator.NameValidator_Length)
		);

		Description = new(GameEditingDataValidator.DescriptionConverter, GameEditingDataValidator.DescriptionInverter, "");

		Version = new(GameEditingDataValidator.VersionConverter, GameEditingDataValidator.VersionInverter,
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, GameEditingDataValidator.VersionNumberComponentInverter, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, GameEditingDataValidator.VersionNumberComponentInverter, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, GameEditingDataValidator.VersionNumberComponentInverter, "0"),
			new SingleInput<string, string, ErrorSeverity>(GameEditingDataValidator.VersionDescriptionConverter, GameEditingDataValidator.VersionDescriptionInverter, "")
		);

		RobotsPerAlliance = new(GameEditingDataValidator.RobotsPerAllianceConverter, GameEditingDataValidator.RobotsPerAllianceInverter, "3");
		AlliancesPerMatch = new(GameEditingDataValidator.AlliancesPerMatchConverter, GameEditingDataValidator.AlliancesPerMatchInverter, "2");

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