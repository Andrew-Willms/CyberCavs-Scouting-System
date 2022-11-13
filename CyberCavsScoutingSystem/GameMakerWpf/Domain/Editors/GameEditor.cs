using System.Collections.ObjectModel;
using System.Linq;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Version = CCSSDomain.Models.Version;

namespace GameMakerWpf.Domain.Editors;



public class GameEditor {

	public SingleInput<string, string, ErrorSeverity> Name { get; }
	public SingleInput<int, string, ErrorSeverity> Year { get; }
	public SingleInput<string, string, ErrorSeverity> Description { get; }

	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version { get; }
	public SingleInput<uint, string, ErrorSeverity> VersionMajorNumber { get; }
	public SingleInput<uint, string, ErrorSeverity> VersionMinorNumber { get; }
	public SingleInput<uint, string, ErrorSeverity> VersionPatchNumber { get; }
	public SingleInput<string, string, ErrorSeverity> VersionDescription { get; }

	public SingleInput<uint, string, ErrorSeverity> RobotsPerAlliance { get; }
	public SingleInput<uint, string, ErrorSeverity> AlliancesPerMatch { get; }

	public ValidationEvent AnythingChanged { get; } = new();
	public ValidationEvent AllianceNameChanged { get; } = new();
	public ValidationEvent AllianceColorChanged { get; } = new();
	public ValidationEvent DataFieldNameChanged { get; } = new();
	public ValidationEvent DataFieldTypeChanged { get; } = new();

	private readonly ObservableCollection<AllianceEditor> _Alliances = new();
	public ReadOnlyObservableCollection<AllianceEditor> Alliances => new(_Alliances);

	private readonly ObservableCollection<DataFieldEditor> _DataFields = new();
	public ReadOnlyObservableCollection<DataFieldEditor> DataFields => new(_DataFields);



	public GameEditor(GameEditingData initialValues) {

		Year = new SingleInputCreator<int, string, ErrorSeverity> { 
			Converter = GameNumbersValidator.YearConverter,
			Inverter =  GameNumbersValidator.YearInverter,
			InitialInput = initialValues.Year
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

		VersionMajorNumber = new SingleInputCreator<uint, string, ErrorSeverity> {
			Converter = GameVersionValidator.ComponentNumberConverter,
			Inverter = GameVersionValidator.ComponentNumberInverter,
			InitialInput = initialValues.VersionMajorNumber
		}.CreateSingleInput();

		VersionMinorNumber = new SingleInputCreator<uint, string, ErrorSeverity> {
			Converter = GameVersionValidator.ComponentNumberConverter,
			Inverter = GameVersionValidator.ComponentNumberInverter,
			InitialInput = initialValues.VersionMinorNumber
		}.CreateSingleInput();

		VersionPatchNumber = new SingleInputCreator<uint, string, ErrorSeverity> {
			Converter = GameVersionValidator.ComponentNumberConverter,
			Inverter = GameVersionValidator.ComponentNumberInverter,
			InitialInput = initialValues.VersionPatchNumber
		}.CreateSingleInput();

		VersionDescription = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = GameVersionValidator.DescriptionConverter,
			Inverter = GameVersionValidator.DescriptionInverter,
			InitialInput = initialValues.VersionDescription
		}.CreateSingleInput();

		Version = new MultiInputCreator<Version, ErrorSeverity, uint, uint, uint, string> {
			Converter = GameVersionValidator.Converter, 
			Inverter = GameVersionValidator.Inverter,
			InputComponent1 = VersionMajorNumber,
			InputComponent2 = VersionMinorNumber,
			InputComponent3 = VersionPatchNumber,
			InputComponent4 = VersionDescription
		}.CreateMultiInput();

		RobotsPerAlliance = new SingleInputCreator<uint, string, ErrorSeverity> {
			Converter = GameNumbersValidator.RobotsPerAllianceConverter,
			Inverter =  GameNumbersValidator.RobotsPerAllianceInverter,
			InitialInput = initialValues.RobotsPerAlliance
		}.CreateSingleInput();

		AlliancesPerMatch = new SingleInputCreator<uint, string, ErrorSeverity> {
			Converter = GameNumbersValidator.AlliancesPerMatchConverter,
			Inverter =  GameNumbersValidator.AlliancesPerMatchInverter,
			InitialInput = initialValues.AlliancesPerMatch
		}.CreateSingleInput();

		foreach (AllianceEditingData allianceEditingData in initialValues.Alliances) {
			AddAlliance(allianceEditingData);
		}

		foreach (DataFieldEditingData dataFieldEditingData in initialValues.DataFields) {
			AddDataField(dataFieldEditingData);
		}

		AnythingChanged.SubscribeTo(Year.OutputObjectChanged);
		AnythingChanged.SubscribeTo(Name.OutputObjectChanged);
		AnythingChanged.SubscribeTo(Description.OutputObjectChanged);
		AnythingChanged.SubscribeTo(Version.OutputObjectChanged);
		AnythingChanged.SubscribeTo(RobotsPerAlliance.OutputObjectChanged);
		AnythingChanged.SubscribeTo(AlliancesPerMatch.OutputObjectChanged);

		AnythingChanged.SubscribeTo(AllianceNameChanged);
		AnythingChanged.SubscribeTo(AllianceColorChanged);
		AnythingChanged.SubscribeTo(DataFieldNameChanged);
		AnythingChanged.SubscribeTo(DataFieldTypeChanged);
	}

	public GameEditingData ToEditingData() {

		return new() {
			Name = Name.InputObject,
			Year = Year.InputObject,
			Description = Description.InputObject,

			VersionMajorNumber = VersionMajorNumber.InputObject,
			VersionMinorNumber = VersionMinorNumber.InputObject,
			VersionPatchNumber = VersionPatchNumber.InputObject,
			VersionDescription = VersionDescription.InputObject,

			RobotsPerAlliance = RobotsPerAlliance.InputObject,
			AlliancesPerMatch = AlliancesPerMatch.InputObject,

			Alliances = Alliances.Select(x => x.ToEditingData()).ToReadOnly(),
			DataFields = DataFields.Select(x => x.ToEditingData()).ToReadOnly()
		};

	}



	public void AddAlliance(AllianceEditingData allianceEditingData) {

		AllianceEditor allianceEditor = new(this, allianceEditingData);

		_Alliances.Add(allianceEditor);
		AllianceNameChanged.SubscribeTo(allianceEditor.Name.OutputObjectChanged);
		AllianceColorChanged.SubscribeTo(allianceEditor.AllianceColor.OutputObjectChanged);

		AllianceNameChanged.Invoke();
		AllianceColorChanged.Invoke();
	}

	public Result<RemoveAllianceError> RemoveAlliance(AllianceEditor allianceEditor) {

		if (!_Alliances.Remove(allianceEditor)) {
			return new RemoveAllianceError { ErrorType = RemoveAllianceError.Types.AllianceNotFound };
		}

		AllianceNameChanged.UnsubscribeFrom(allianceEditor.Name.OutputObjectChanged);
		AllianceColorChanged.UnsubscribeFrom(allianceEditor.AllianceColor.OutputObjectChanged);

		AllianceNameChanged.Invoke();
		AllianceColorChanged.Invoke();

		return new Success();
	}

	public void AddDataField(DataFieldEditingData dataFieldEditingData) {

		DataFieldEditor dataFieldEditor = new(this, dataFieldEditingData);

		_DataFields.Add(dataFieldEditor);
		DataFieldNameChanged.SubscribeTo(dataFieldEditor.Name.OutputObjectChanged);
		DataFieldTypeChanged.SubscribeTo(dataFieldEditor.Name.OutputObjectChanged);

		DataFieldNameChanged.Invoke();
		DataFieldTypeChanged.Invoke();
	}

	public Result<RemoveDataFieldError> RemoveDataField(DataFieldEditor dataFieldEditor) {

		if (!_DataFields.Remove(dataFieldEditor)) {
			return new RemoveDataFieldError { ErrorType = RemoveDataFieldError.Types.DataFieldNotFound };
		}
		
		DataFieldNameChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);
		DataFieldTypeChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);

		DataFieldNameChanged.Invoke();
		DataFieldTypeChanged.Invoke();

		return new Success();
	}



	public class RemoveAllianceError : Error<RemoveAllianceError.Types> {

		public enum Types {
			AllianceNotFound
		}

	}

	public class RemoveDataFieldError : Error<RemoveDataFieldError.Types> {

		public enum Types {
			DataFieldNotFound
		}

	}

}