using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

using WPFUtilities;

namespace CCSSDomain;



public class GameEditingData : INotifyPropertyChanged {

	private GameEditingDataValidator Validator { get; }

	public GameEditingData() {

		Validator = new(this);

		Year = new(Validator.YearValidator, DateTime.Now.Year.ToString());
		Name = new(Validator.NameValidator, "");
		Description = new(Validator.DescriptionValidator, "");

		Version = new(Validator.VersionCovalidator,
			(nameof(VersionNumber.MajorNumber), new StringInput<int, ErrorSeverity>(Validator.TestIntValueConverter, "1")),
			(nameof(VersionNumber.MinorNumber), new StringInput<int, ErrorSeverity>(Validator.TestIntValueConverter, "2")),
			(nameof(VersionNumber.PatchNumber), new StringInput<int, ErrorSeverity>(Validator.TestIntValueConverter, "3")),
			(nameof(VersionNumber.VersionDescription), new StringInput<string, ErrorSeverity>(Validator.VersionDescriptionValidator, ""))
		);

		RobotsPerAlliance = new(Validator.TestIntValueConverter, "3");
		AlliancesPerMatch = new(Validator.TestIntValueConverter, "2");

		Alliances = new() {
			new(),
			new()
		};
	}



	public StringInput<string, ErrorSeverity> Name { get; }
	public StringInput<int, ErrorSeverity> Year { get; }
	public StringInput<string, ErrorSeverity> Description { get; }
	public MultiStringInput<VersionNumber, ErrorSeverity> Version { get; }

	//public DateTime VersionReleaseDate { get; set; }
	// Figure if/how to do version history later as it's not critical.
	//public List<VersionNumber, string, DateTime> VersionHistory;

	public StringInput<int, ErrorSeverity> RobotsPerAlliance { get; }
	public StringInput<int, ErrorSeverity> AlliancesPerMatch { get; }

	public ObservableCollection<AllianceEditingData> Alliances { get; }


	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}