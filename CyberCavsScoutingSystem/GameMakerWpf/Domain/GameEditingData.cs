using System.Collections.ObjectModel;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Version = CCSSDomain.Version;

namespace GameMakerWpf.Domain;



public class GameEditingData {

	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }
	public SingleInput<int, string, ErrorSeverity> Year { get; }
	public SingleInput<string, string, ErrorSeverity> Description { get; }

	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance { get; }
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AllianceNameChanged { get; } = new();
	public ValidationEvent AllianceColorChanged { get; } = new();
	public ValidationEvent DataFieldNameChanged { get; } = new();

	private readonly ObservableCollection<AllianceEditingData> _Alliances = new();
	public ReadOnlyObservableCollection<AllianceEditingData> Alliances => new(_Alliances);


	private readonly ObservableCollection<DataFieldEditingData> _DataFields = new();
	public ReadOnlyObservableCollection<DataFieldEditingData> DataFields => new(_DataFields);





	public GameEditingData(Game initialValues) {

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

		foreach (Alliance alliance in initialValues.Alliances) {
			AddAlliance(new(this, alliance));
		}

		foreach (DataField dataField in initialValues.DataFields) {
			AddDataField(DataFieldEditingData.DataFieldEditingDataFromDataField(dataField, this));
		}
	}



	public void AddAlliance(AllianceEditingData allianceEditingData) {

		_Alliances.Add(allianceEditingData);
		AllianceNameChanged.SubscribeTo(allianceEditingData.Name.OutputObjectChanged);
		AllianceColorChanged.SubscribeTo(allianceEditingData.AllianceColor.OutputObjectChanged);
	}

	public void RemoveAlliance(AllianceEditingData allianceEditingData) {

		_Alliances.Remove(allianceEditingData);
		AllianceNameChanged.UnsubscribeFrom(allianceEditingData.Name.OutputObjectChanged);
		AllianceColorChanged.UnsubscribeFrom(allianceEditingData.AllianceColor.OutputObjectChanged);
	}

	public void AddDataField(DataFieldEditingData dataFieldEditingData) {

		_DataFields.Add(dataFieldEditingData);
		DataFieldNameChanged.SubscribeTo(dataFieldEditingData.Name.OutputObjectChanged);
	}

	public void RemoveDataField(DataFieldEditingData dataFieldEditingData) {

		_DataFields.Remove(dataFieldEditingData);
		DataFieldNameChanged.UnsubscribeFrom(dataFieldEditingData.Name.OutputObjectChanged);
	}

}