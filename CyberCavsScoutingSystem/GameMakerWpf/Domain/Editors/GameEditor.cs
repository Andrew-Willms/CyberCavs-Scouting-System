﻿using System.Diagnostics;
using System.Linq;
using CCSSDomain;
using CCSSDomain.GameSpecification;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.SimpleEvent;
using UtilitiesLibrary.Validation.Inputs;
using Version = CCSSDomain.GameSpecification.Version;

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

	public ObservableList<AllianceEditor, AllianceEditingData> Alliances { get; }
	public ObservableList<DataFieldEditor, DataFieldEditingData> DataFields { get; }

	public ObservableList<InputEditor, InputEditingData> SetupTabInputs { get; }
	public ObservableList<InputEditor, InputEditingData> AutoTabInputs { get; }
	public ObservableList<InputEditor, InputEditingData> TeleTabInputs { get; }
	public ObservableList<InputEditor, InputEditingData> EndgameTabInputs { get; }

	public ObservableList<ButtonEditor, ButtonEditingData> AutoButtons { get; }
	public ObservableList<ButtonEditor, ButtonEditingData> TeleButtons { get; }



	public GameEditor(GameEditingData initialValues) {

		Alliances = new() {
			Adder = allianceEditingData => new(this, allianceEditingData),
			OnAdd = allianceEditor => {
				AllianceNameChanged.SubscribeTo(allianceEditor.Name.OutputObjectChanged);
				AllianceColorChanged.SubscribeTo(allianceEditor.Color.OutputObjectChanged);
				AllianceNameChanged.Invoke();
				AllianceColorChanged.Invoke();
			},
			OnRemove = allianceEditor => {
				AllianceNameChanged.UnsubscribeFrom(allianceEditor.Name.OutputObjectChanged);
				AllianceColorChanged.UnsubscribeFrom(allianceEditor.Color.OutputObjectChanged);
				AllianceNameChanged.Invoke();
				AllianceColorChanged.Invoke();
			}
		};

		DataFields = new() {
			Adder = dataFieldEditingData => new(this, dataFieldEditingData),
			OnAdd = dataFieldEditor => {
				DataFieldNameChanged.SubscribeTo(dataFieldEditor.Name.OutputObjectChanged);
				DataFieldTypeChanged.SubscribeTo(dataFieldEditor.Name.OutputObjectChanged);
				DataFieldNameChanged.Invoke();
				DataFieldTypeChanged.Invoke();
			},
			OnRemove = dataFieldEditor => {
				DataFieldNameChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);
				DataFieldTypeChanged.UnsubscribeFrom(dataFieldEditor.Name.OutputObjectChanged);
				DataFieldNameChanged.Invoke();
				DataFieldTypeChanged.Invoke();
			}
		};

		SetupTabInputs = new() {
			Adder = inputEditingData => new(this, inputEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

		AutoTabInputs = new() {
			Adder = inputEditingData => new(this, inputEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

		TeleTabInputs = new() {
			Adder = inputEditingData => new(this, inputEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

		EndgameTabInputs = new() {
			Adder = inputEditingData => new(this, inputEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

		AutoButtons = new() {
			Adder = buttonEditingData => new(this, buttonEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

		TeleButtons = new() {
			Adder = buttonEditingData => new(this, buttonEditingData),
			OnAdd = _ => AnythingChanged.Invoke(),
			OnRemove = _ => AnythingChanged.Invoke()
		};

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

		initialValues.Alliances.Foreach(Alliances.Add);
		initialValues.DataFields.Foreach(DataFields.Add);

		initialValues.SetupTabInputs.Foreach(SetupTabInputs.Add);
		initialValues.AutoTabInputs.Foreach(AutoTabInputs.Add);
		initialValues.TeleTabInputs.Foreach(TeleTabInputs.Add);
		initialValues.EndgameTabInputs.Foreach(EndgameTabInputs.Add);

		initialValues.AutoButtons.Foreach(AutoButtons.Add);
		initialValues.TeleButtons.Foreach(TeleButtons.Add);

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

			AutoButtons = AutoButtons.Select(x => x.ToEditingData()).ToReadOnly(),
			TeleButtons = TeleButtons.Select(x => x.ToEditingData()).ToReadOnly(),
		};

	}

	public bool IsValid => Name.IsValid && 
	                       Year.IsValid && 
	                       Description.IsValid &&
	                       Version.IsValid &&
	                       RobotsPerAlliance.IsValid &&
	                       AlliancesPerMatch.IsValid &&
	                       Alliances.All(x => x.IsValid) &&
	                       DataFields.All(x => x.IsValid) &&
	                       SetupTabInputs.All(x => x.IsValid) &&
	                       AutoTabInputs.All(x => x.IsValid) &&
	                       TeleTabInputs.All(x => x.IsValid) &&
	                       EndgameTabInputs.All(x => x.IsValid) &&
	                       AutoButtons.All(x => x.IsValid) &&
	                       TeleButtons.All(x => x.IsValid);

	public IEditorToGameSpecificationResult<Game> ToGameSpecification() {

		if (!IsValid) {
			return new IEditorToGameSpecificationResult<Game>.EditorIsInvalid();
		}

		return new IEditorToGameSpecificationResult<Game>.Success { Value = new() {

			Name = Name.OutputObject.Value,
			Year = Year.OutputObject.Value,
			Description = Description.OutputObject.Value,
			Version = Version.OutputObject.Value,
			RobotsPerAlliance = RobotsPerAlliance.OutputObject.Value,
			AlliancesPerMatch = AlliancesPerMatch.OutputObject.Value,

			Alliances = Alliances.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Alliance>.Success)?.Value??
				throw new UnreachableException()).ToReadOnly(),

			DataFields = DataFields.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<DataField>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			SetupTabInputs = SetupTabInputs.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Input>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			AutoTabInputs = AutoTabInputs.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Input>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			TeleTabInputs = TeleTabInputs.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Input>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			EndgameTabInputs = EndgameTabInputs.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Input>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			AutoButtons = AutoButtons.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Button>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),

			TeleButtons = TeleButtons.Select(x =>
				(x.ToGameSpecification() as IEditorToGameSpecificationResult<Button>.Success)?.Value ??
				throw new UnreachableException()).ToReadOnly(),
		}};
	}

}



public interface IEditorToGameSpecificationResult<TGameSpecification> : IResult<TGameSpecification> {

	public class Success : IResult<TGameSpecification>.Success, IEditorToGameSpecificationResult<TGameSpecification> { }

	public class EditorIsInvalid : Error, IEditorToGameSpecificationResult<TGameSpecification> { }

}