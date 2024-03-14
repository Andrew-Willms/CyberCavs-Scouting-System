using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CCSSDomain.GameSpecification;
using ExhaustiveMatching;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Inputs;
using static CCSSDomain.GameSpecification.DataFieldSpec;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



public class DataFieldEditor : INotifyPropertyChanged {

	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public DataFieldTypeEditor DataFieldTypeEditor { get; private set; }

	private DataFieldType _DataFieldType;
	public DataFieldType DataFieldType {
		get => _DataFieldType;
		set {

			if (_DataFieldType == value) {
				return;
			}

			_DataFieldType = value;
			ChangeDataFieldType(value);
			OnPropertyChanged(nameof(DataFieldType));
			OnPropertyChanged(nameof(DataFieldTypeEditor));
		}
	}



	public DataFieldEditor(GameEditor gameEditor, DataFieldEditingData initialValues) {

		GameEditor = gameEditor;

		DataFieldType = initialValues.DataFieldType;

		DataFieldTypeEditor = initialValues switch {
			BooleanDataFieldEditingData booleanDataFieldEditingData => new BooleanDataFieldEditor(GameEditor, booleanDataFieldEditingData),
			TextDataFieldEditingData textDataFieldEditingData => new TextDataFieldEditor(GameEditor, textDataFieldEditingData),
			IntegerDataFieldEditingData integerDataFieldEditingData => new IntegerDataFieldEditor(GameEditor, integerDataFieldEditingData),
			SelectionDataFieldEditingData selectionDataFieldEditingData => new SelectionDataFieldEditor(GameEditor, selectionDataFieldEditingData),
			_ => throw ExhaustiveMatch.Failed(initialValues)
		};

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = DataFieldValidator.NameConverter,
			Inverter = DataFieldValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddValidationRule(DataFieldValidator.NameValidator_Length)
		.AddValidationRule<IEnumerable<DataFieldEditor>>(DataFieldValidator.NameValidator_Uniqueness, () => GameEditor.DataFields, false, GameEditor.DataFieldNameChanged)
		.CreateSingleInput();

		Name.OutputObjectChanged.Subscribe(GameEditor.AnythingChanged.Invoke);
	}

	private void ChangeDataFieldType(DataFieldType dataFieldType) {

		DataFieldTypeEditor = dataFieldType switch {
			DataFieldType.Boolean => new BooleanDataFieldEditor(GameEditor, DefaultEditingDataValues.DefaultBooleanDataFieldEditingData),
			DataFieldType.Text => new TextDataFieldEditor(GameEditor, DefaultEditingDataValues.DefaultTextDataFieldEditingData),
			DataFieldType.Selection => new SelectionDataFieldEditor(GameEditor, DefaultEditingDataValues.DefaultSelectionDataFieldEditingData),
			DataFieldType.Integer => new IntegerDataFieldEditor(GameEditor, DefaultEditingDataValues.DefaultIntegerDataFieldEditingData),
			_ => throw new UnreachableException()
		};

		GameEditor.DataFieldTypeChanged.Invoke();
		GameEditor.AnythingChanged.Invoke();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}



	public DataFieldEditingData ToEditingData() {

		return DataFieldTypeEditor switch {

			BooleanDataFieldEditor booleanDataFieldEditor => new BooleanDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Boolean,
				InitialValue = booleanDataFieldEditor.InitialValue.InputObject,
			},

			TextDataFieldEditor textDataFieldEditor => new TextDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Text,
				InitialValue = textDataFieldEditor.InitialValue.InputObject,
				MustNotBeEmpty = textDataFieldEditor.MustNotBeInitialValue.InputObject,
				MustNotBeInitialValue = textDataFieldEditor.MustNotBeInitialValue.InputObject
			},

			IntegerDataFieldEditor integerDataFieldEditor => new IntegerDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Integer,
				InitialValue = integerDataFieldEditor.InitialValue.InputObject,
				MinValue = integerDataFieldEditor.MinValue.InputObject,
				MaxValue = integerDataFieldEditor.MaxValue.InputObject
			},

			SelectionDataFieldEditor selectionDataFieldEditor => new SelectionDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Selection,
				OptionNames = selectionDataFieldEditor.Options.Select(x => x.InputObject).ToReadOnly()
			},

			_ => throw ExhaustiveMatch.Failed(DataFieldTypeEditor)
		};
	}

	public bool IsValid => Name.IsValid && DataFieldTypeEditor switch {
		BooleanDataFieldEditor => true,
		TextDataFieldEditor => true,
		IntegerDataFieldEditor integerDataFieldEditor =>
			integerDataFieldEditor.InitialValue.IsValid &&
		    integerDataFieldEditor.MinValue.IsValid &&
		    integerDataFieldEditor.MaxValue.IsValid,
		SelectionDataFieldEditor selectionDataFieldEditor => selectionDataFieldEditor.Options.All(x => x.IsValid),
		_ => throw ExhaustiveMatch.Failed(DataFieldTypeEditor)
	};

	public DataFieldSpec? ToGameSpecification() {

		if (!IsValid) {
			return null;
		}

		return DataFieldTypeEditor switch {

			BooleanDataFieldEditor booleanDataFieldEditor => new BooleanDataFieldSpec {
				Name = Name.InputObject,
				InitialValue = booleanDataFieldEditor.InitialValue.OutputObject.Value
			},

			TextDataFieldEditor textDataFieldEditor => new TextDataFieldSpec {
				Name = Name.InputObject,
				InitialValue = textDataFieldEditor.InitialValue.OutputObject.Value,
				MustNotBeEmpty = textDataFieldEditor.MustNotBeEmpty.OutputObject.Value,
				MustNotBeInitialValue = textDataFieldEditor.MustNotBeInitialValue.OutputObject.Value
			},

			IntegerDataFieldEditor integerDataFieldEditor => new IntegerDataFieldSpec {
				Name = Name.InputObject,
				InitialValue = integerDataFieldEditor.InitialValue.OutputObject.Value,
				MinValue = integerDataFieldEditor.MinValue.OutputObject.Value,
				MaxValue = integerDataFieldEditor.MaxValue.OutputObject.Value
			},

			SelectionDataFieldEditor selectionDataFieldEditor => new SelectionDataFieldSpec {
				Name = Name.InputObject,
				OptionNames = selectionDataFieldEditor.Options.Select(x => x.OutputObject.Value).ToReadOnly()
			},

			_ => throw ExhaustiveMatch.Failed(DataFieldTypeEditor)
		};
	}

}