using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFUtilities.Validation;

namespace CCSSDomain;



public class GameEditingData : INotifyPropertyChanged {

	private GameEditingData() {

		Year = new(GameEditingDataValidator.YearConverter,
			DateTime.Now.Year.ToString(),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotNegative),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotFarFuture),
			new ValidationSet<int, ErrorSeverity>(GameEditingDataValidator.YearValidator_YearNotPredateFirst)
		);

		Name = new(GameEditingDataValidator.NameConverter,
			DateTime.Now.Year.ToString(),
			new ValidationSet<string, ErrorSeverity>(GameEditingDataValidator.NameValidator_Length)
		);

		Description = new(GameEditingDataValidator.DescriptionConverter, "");

		Version = new(GameEditingDataValidator.VersionConverter,
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, "0"),
			new SingleInput<uint, string, ErrorSeverity>(GameEditingDataValidator.VersionNumberComponentConverter, "0"),
			new SingleInput<string, string, ErrorSeverity>(GameEditingDataValidator.VersionDescriptionConverter, "0")
		);

		RobotsPerAlliance = new(GameEditingDataValidator.TestIntValueConverter, "3");
		AlliancesPerMatch = new(GameEditingDataValidator.TestIntValueConverter, "2");

		Alliances = new();
	}



	public static GameEditingData GetDefaultEditingData() {

		GameEditingData gameEditingData = new();

		gameEditingData.Alliances.Add(new(gameEditingData));
		gameEditingData.Alliances.Add(new(gameEditingData));

		gameEditingData.Alliances[0].Name.InputString = "Red Alliance";
		gameEditingData.Alliances[1].Name.InputString = "Blue Alliance";
		
		gameEditingData.Alliances[0].AllianceColor.InputComponent1.InputString = "255";
		gameEditingData.Alliances[0].AllianceColor.StringInputs["G"].InputString = "0";
		gameEditingData.Alliances[0].AllianceColor.StringInputs["B"].InputString = "0";
		
		gameEditingData.Alliances[1].AllianceColor.StringInputs["R"].InputString = "0";
		gameEditingData.Alliances[1].AllianceColor.StringInputs["G"].InputString = "0";
		gameEditingData.Alliances[1].AllianceColor.StringInputs["B"].InputString = "255";

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