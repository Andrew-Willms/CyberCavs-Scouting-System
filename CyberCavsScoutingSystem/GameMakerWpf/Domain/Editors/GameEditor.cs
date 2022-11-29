using System.Collections.ObjectModel;
using System.Linq;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary;
using UtilitiesLibrary.Collections;
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

	public Event AnythingChanged { get; } = new();
	public Event AllianceNameChanged { get; } = new();
	public Event AllianceColorChanged { get; } = new();
	public Event DataFieldNameChanged { get; } = new();
	public Event DataFieldTypeChanged { get; } = new();

	private readonly ObservableCollection<AllianceEditor> _Alliances = new();
	public ReadOnlyObservableCollection<AllianceEditor> Alliances => new(_Alliances);

	private readonly ObservableCollection<DataFieldEditor> _DataFields = new();
	public ReadOnlyObservableCollection<DataFieldEditor> DataFields => new(_DataFields);

	private readonly ObservableCollection<InputEditor> _SetupTabInputs = new();
	public ReadOnlyObservableCollection<InputEditor> SetupTabInputs => new(_SetupTabInputs);

	private readonly ObservableCollection<InputEditor> _AutoTabInputs = new();
	public ReadOnlyObservableCollection<InputEditor> AutoTabInputs => new(_AutoTabInputs);

	private readonly ObservableCollection<InputEditor> _TeleTabInputs = new();
	public ReadOnlyObservableCollection<InputEditor> TeleTabInputs => new(_TeleTabInputs);

	private readonly ObservableCollection<InputEditor> _EndgameTabInputs = new();
	public ReadOnlyObservableCollection<InputEditor> EndgameTabInputs => new(_EndgameTabInputs);

	private readonly ObservableCollection<ButtonEditor> _AutoButtons = new();
	public ReadOnlyObservableCollection<ButtonEditor> AutoButtons => new(_AutoButtons);

	private readonly ObservableCollection<ButtonEditor> _TeleButtons = new();
	public ReadOnlyObservableCollection<ButtonEditor> TeleButtons => new(_TeleButtons);



	public GameEditor(GameEditingData initialValues) {

		Year = new SingleInputCreator<int, string, ErrorSeverity> { 
			Converter = GameNumbersValidator.YearConverter,
			Inverter =  GameNumbersValidator.YearInverter,
			InitialInput = initialValues.Year
		}.AddValidationRule(GameNumbersValidator.YearValidator_YearNotNegative)
		.AddValidationRule(GameNumbersValidator.YearValidator_YearNotFarFuture)
		.AddValidationRule(GameNumbersValidator.YearValidator_YearNotPredateFirst)
		.CreateSingleInput();

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = GameTextValidator.NameConverter,
			Inverter = GameTextValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddValidationRule(GameTextValidator.NameValidator_Length)
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

		initialValues.Alliances.Foreach(AddAlliance);
		initialValues.DataFields.Foreach(AddDataField);

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
			DataFields = DataFields.Select(x => x.ToEditingData()).ToReadOnly(),

			SetupTabInputs = SetupTabInputs.Select(x => x.ToEditingData()).ToReadOnly(),
			AutoTabInputs = AutoTabInputs.Select(x => x.ToEditingData()).ToReadOnly(),
			TeleTabInputs = TeleTabInputs.Select(x => x.ToEditingData()).ToReadOnly(),
			EndgameTabInputs = EndgameTabInputs.Select(x => x.ToEditingData()).ToReadOnly(),

			AutoButtonEditingData = AutoButtons.Select(x => x.ToEditingData()).ToReadOnly(),
			TeleButtonEditingData = TeleButtons.Select(x => x.ToEditingData()).ToReadOnly(),
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

	public Result<RemoveError> RemoveAlliance(AllianceEditor allianceEditor) {

		if (!_Alliances.Remove(allianceEditor)) {
			return RemoveError.NotFound;
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

	public Result<RemoveError> RemoveDataField(DataFieldEditor dataFieldEditor) {

		if (!_DataFields.Remove(dataFieldEditor)) {
			return RemoveError.NotFound;
		}
		
		DataFieldNameChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);
		DataFieldTypeChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);

		DataFieldNameChanged.Invoke();
		DataFieldTypeChanged.Invoke();

		return new Success();
	}



	public void AddSetupTabInput(InputEditingData inputEditingData) {

		InputEditor inputEditor = new(this, inputEditingData);
		_SetupTabInputs.Add(inputEditor);
	}

	public Result<RemoveError> RemoveSetupInput(InputEditor inputEditor) {

		if (!_SetupTabInputs.Remove(inputEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}

	public void AddAutoTabInput(InputEditingData inputEditingData) {

		InputEditor inputEditor = new(this, inputEditingData);
		_AutoTabInputs.Add(inputEditor);
	}

	public Result<RemoveError> RemoveAutoInput(InputEditor inputEditor) {

		if (!_AutoTabInputs.Remove(inputEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}

	public void AddTeleTabInput(InputEditingData inputEditingData) {

		InputEditor inputEditor = new(this, inputEditingData);
		_TeleTabInputs.Add(inputEditor);
	}

	public Result<RemoveError> RemoveTeleInput(InputEditor inputEditor) {

		if (!_TeleTabInputs.Remove(inputEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}

	public void AddEndgameTabInput(InputEditingData inputEditingData) {

		InputEditor inputEditor = new(this, inputEditingData);
		_EndgameTabInputs.Add(inputEditor);
	}

	public Result<RemoveError> RemoveEndgameInput(InputEditor inputEditor) {

		if (!_EndgameTabInputs.Remove(inputEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}



	public void AddAutoButton(ButtonEditingData buttonEditingData) {

		ButtonEditor buttonEditor = new(this, buttonEditingData);
		_AutoButtons.Add(buttonEditor);
	}

	public Result<RemoveError> RemoveAutoButton(ButtonEditor buttonEditor) {

		if (!_AutoButtons.Remove(buttonEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}

	public void AddTeleButton(ButtonEditingData buttonEditingData) {

		ButtonEditor buttonEditor = new(this, buttonEditingData);

		_TeleButtons.Add(buttonEditor);
	}

	public Result<RemoveError> RemoveTeleButton(ButtonEditor buttonEditor) {

		if (!_TeleButtons.Remove(buttonEditor)) {
			return RemoveError.NotFound;
		}

		return new Success();
	}



	public class RemoveError : Error<RemoveError.Types> {

		public enum Types {
			ItemNotFound
		}

		public static RemoveError NotFound => new() { ErrorType = Types.ItemNotFound };

	}

}