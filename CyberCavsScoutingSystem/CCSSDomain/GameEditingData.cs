using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Reflection;
using WPFUtilities.Validation;

namespace CCSSDomain;



public class GameEditingData : INotifyPropertyChanged {

	private GameEditingData() {

		Year = new(GameEditingDataValidator.YearConverter, DateTime.Now.Year.ToString(), new());

		Year = new(GameEditingDataValidator.YearValidator, DateTime.Now.Year.ToString());
		Name = new(GameEditingDataValidator.NameValidator, "");
		Description = new(GameEditingDataValidator.DescriptionValidator, "");

		Version = new(GameEditingDataValidator.VersionCovalidator,
			(nameof(VersionNumber.MajorNumber), new StringInput<int, ErrorSeverity>(GameEditingDataValidator.TestIntValueConverter, "1")),
			(nameof(VersionNumber.MinorNumber), new StringInput<int, ErrorSeverity>(GameEditingDataValidator.TestIntValueConverter, "2")),
			(nameof(VersionNumber.PatchNumber), new StringInput<int, ErrorSeverity>(GameEditingDataValidator.TestIntValueConverter, "3")),
			(nameof(VersionNumber.VersionDescription), new StringInput<string, ErrorSeverity>(GameEditingDataValidator.VersionDescriptionValidator, ""))
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
		
		gameEditingData.Alliances[0].AllianceColor.StringInputs["R"].InputString = "255";
		gameEditingData.Alliances[0].AllianceColor.StringInputs["G"].InputString = "0";
		gameEditingData.Alliances[0].AllianceColor.StringInputs["B"].InputString = "0";
		
		gameEditingData.Alliances[1].AllianceColor.StringInputs["R"].InputString = "0";
		gameEditingData.Alliances[1].AllianceColor.StringInputs["G"].InputString = "0";
		gameEditingData.Alliances[1].AllianceColor.StringInputs["B"].InputString = "255";

		return gameEditingData;
	}



	public StringInput<string, ErrorSeverity> Name { get; }
	public StringInput<int, ErrorSeverity> Year { get; }
	public StringInput<string, ErrorSeverity> Description { get; }
	public MultiInput<VersionNumber, ErrorSeverity> Version { get; }

	//public DateTime VersionReleaseDate { get; set; }
	// Figure if/how to do version history later as it's not critical.
	//public List<VersionNumber, string, DateTime> VersionHistory;

	public StringInput<int, ErrorSeverity> RobotsPerAlliance { get; }
	public StringInput<int, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AllianceNameChanged { get; } = new();

	public ObservableCollection<AllianceEditingData> Alliances { get; } // = new();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}