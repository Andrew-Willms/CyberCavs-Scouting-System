using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Version = CCSSDomain.Version;

namespace GameMakerWpf.Domain;



public class GameEditingData : INotifyPropertyChanged {

	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }
	public SingleInput<int, string, ErrorSeverity> Year { get; }
	public SingleInput<string, string, ErrorSeverity> Description { get; }

	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance { get; }
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AllianceNameChanged { get; } = new();

	public ObservableCollection<AllianceEditingData> Alliances { get; }



	public GameEditingData(Game? initialValues = null) {

		initialValues ??= new();

		Year = new(GameNumbersValidator.YearConverter, GameNumbersValidator.YearInverter, initialValues.Year.ToString(),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotNegative),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotFarFuture),
			new ValidationSet<int, ErrorSeverity>(GameNumbersValidator.YearValidator_YearNotPredateFirst)
		);

		Name = new(GameTextValidator.NameConverter, GameTextValidator.NameInverter, initialValues.Name,
			new ValidationSet<string, ErrorSeverity>(GameTextValidator.NameValidator_Length)
		);

		Description = new(GameTextValidator.DescriptionConverter, GameTextValidator.DescriptionInverter, initialValues.Description);

		Version = new(GameVersionValidator.Converter, GameVersionValidator.Inverter,

			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter,
				GameVersionValidator.ComponentNumberInverter, initialValues.Version.MajorNumber.ToString()),

			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter,
				GameVersionValidator.ComponentNumberInverter, initialValues.Version.MinorNumber.ToString()),

			new SingleInput<uint, string, ErrorSeverity>(GameVersionValidator.ComponentNumberConverter,
				GameVersionValidator.ComponentNumberInverter, initialValues.Version.PatchNumber.ToString()),

			new SingleInput<string, string, ErrorSeverity>(GameVersionValidator.DescriptionConverter,
				GameVersionValidator.DescriptionInverter, initialValues.Version.Description)
		);

		RobotsPerAlliance = new(GameNumbersValidator.RobotsPerAllianceConverter, GameNumbersValidator.RobotsPerAllianceInverter,
			initialValues.RobotsPerAlliance.ToString());

		AlliancesPerMatch = new(GameNumbersValidator.AlliancesPerMatchConverter, GameNumbersValidator.AlliancesPerMatchInverter,
			initialValues.AlliancesPerMatch.ToString());

		Alliances = new(initialValues.Alliances.Select(x => new AllianceEditingData(this, x)));
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}