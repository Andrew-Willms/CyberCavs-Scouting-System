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

		Year = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = GameNumbersValidator.YearConverter,
				Inverter =  GameNumbersValidator.YearInverter,
				InitialInput = initialValues.Year.ToString()
			}.AddOnChangeValidator(GameNumbersValidator.YearValidator_YearNotNegative)
			.AddOnChangeValidator(GameNumbersValidator.YearValidator_YearNotFarFuture)
			.AddOnChangeValidator(GameNumbersValidator.YearValidator_YearNotPredateFirst)
			.CreateSingleInput();

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
				Converter = GameTextValidator.NameConverter,
				Inverter = GameTextValidator.NameInverter,
				InitialInput = initialValues.Name
			}.AddOnChangeValidator(GameTextValidator.NameValidator_Length)
			.CreateSingleInput();

		Description = new SingleInputCreator<string, string, ErrorSeverity> {
				Converter = GameTextValidator.DescriptionConverter,
				Inverter = GameTextValidator.DescriptionInverter,
				InitialInput = initialValues.Description
			}.CreateSingleInput();


		Version = new MultiInputCreator<Version, ErrorSeverity, uint, uint, uint, string> {

				Converter = GameVersionValidator.Converter, 
				Inverter = GameVersionValidator.Inverter,

				InputComponent1 = new SingleInputCreator<uint, string, ErrorSeverity> {
					Converter = GameVersionValidator.ComponentNumberConverter,
					Inverter =  GameVersionValidator.ComponentNumberInverter,
					InitialInput = initialValues.Version.MajorNumber.ToString()
				}.CreateSingleInput(),

				InputComponent2 = new SingleInputCreator<uint, string, ErrorSeverity> {
					Converter = GameVersionValidator.ComponentNumberConverter,
					Inverter =  GameVersionValidator.ComponentNumberInverter,
					InitialInput = initialValues.Version.MinorNumber.ToString()
				}.CreateSingleInput(),

				InputComponent3 = new SingleInputCreator<uint, string, ErrorSeverity> {
					Converter = GameVersionValidator.ComponentNumberConverter,
					Inverter =  GameVersionValidator.ComponentNumberInverter,
					InitialInput = initialValues.Version.PatchNumber.ToString()
				}.CreateSingleInput(),

				InputComponent4 = new SingleInputCreator<string, string, ErrorSeverity> {
					Converter = GameVersionValidator.DescriptionConverter,
					Inverter =  GameVersionValidator.DescriptionInverter,
					InitialInput = initialValues.Version.Description
				}.CreateSingleInput()

			}.CreateSingleInput();

		RobotsPerAlliance = new SingleInputCreator<uint, string, ErrorSeverity> {
				Converter = GameNumbersValidator.RobotsPerAllianceConverter,
				Inverter =  GameNumbersValidator.RobotsPerAllianceInverter,
				InitialInput = initialValues.RobotsPerAlliance.ToString()
			}.CreateSingleInput();

		AlliancesPerMatch = new SingleInputCreator<uint, string, ErrorSeverity> {
				Converter = GameNumbersValidator.AlliancesPerMatchConverter,
				Inverter =  GameNumbersValidator.AlliancesPerMatchInverter,
				InitialInput = initialValues.AlliancesPerMatch.ToString()
			}.CreateSingleInput();

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

		AllianceNameChanged.Invoke();
		AllianceColorChanged.Invoke();
	}

	public void RemoveAlliance(AllianceEditingData allianceEditingData) {

		_Alliances.Remove(allianceEditingData);
		AllianceNameChanged.UnsubscribeFrom(allianceEditingData.Name.OutputObjectChanged);
		AllianceColorChanged.UnsubscribeFrom(allianceEditingData.AllianceColor.OutputObjectChanged);

		AllianceNameChanged.Invoke();
		AllianceColorChanged.Invoke();
	}

	public void AddDataField(DataFieldEditingData dataFieldEditingData) {

		_DataFields.Add(dataFieldEditingData);
		DataFieldNameChanged.SubscribeTo(dataFieldEditingData.Name.OutputObjectChanged);

		DataFieldNameChanged.Invoke();
	}

	public void RemoveDataField(DataFieldEditingData dataFieldEditingData) {

		_DataFields.Remove(dataFieldEditingData);
		DataFieldNameChanged.UnsubscribeFrom(dataFieldEditingData.Name.OutputObjectChanged);

		DataFieldNameChanged.Invoke();
	}

}