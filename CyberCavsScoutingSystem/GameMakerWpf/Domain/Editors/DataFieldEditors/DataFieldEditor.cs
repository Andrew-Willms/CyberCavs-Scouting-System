using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CCSSDomain;
using CCSSDomain.GameSpecification;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Inputs;
using static CCSSDomain.GameSpecification.DataField;

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
			TextDataFieldEditingData => new TextDataFieldEditor(),
			SelectionDataFieldEditingData selectionDataField => new SelectionDataFieldEditor(GameEditor, selectionDataField),
			IntegerDataFieldEditingData integerDataField => new IntegerDataFieldEditor(GameEditor, integerDataField),
			_ => throw new UnreachableException()
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
			DataFieldType.Text => new TextDataFieldEditor(),
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

			TextDataFieldEditor => new TextDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Text
			},

			SelectionDataFieldEditor selectionDataFieldEditor => new SelectionDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Selection,
				OptionNames = selectionDataFieldEditor.Options.Select(x => x.InputObject).ToReadOnly()
			},

			IntegerDataFieldEditor integerDataFieldEditor => new IntegerDataFieldEditingData {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Integer,
				InitialValue = integerDataFieldEditor.InitialValue.InputObject,
				MinValue = integerDataFieldEditor.MinValue.InputObject,
				MaxValue = integerDataFieldEditor.MaxValue.InputObject
			},

			_ => throw new UnreachableException()
		};
	}

	public bool IsValid {
		get {
			return Name.IsValid && DataFieldTypeEditor switch {

				TextDataFieldEditor => true,

				SelectionDataFieldEditor selectionDataFieldEditor => selectionDataFieldEditor.Options.All(x => x.IsValid),

				IntegerDataFieldEditor integerDataFieldEditor => integerDataFieldEditor.InitialValue.IsValid &&
				                                                 integerDataFieldEditor.MinValue.IsValid &&
				                                                 integerDataFieldEditor.MaxValue.IsValid,

				_ => throw new UnreachableException()
			};

		}
	}

	public IEditorToGameSpecificationResult<DataField> ToGameSpecification() {

		if (!IsValid) {
			return new IEditorToGameSpecificationResult<DataField>.EditorIsInvalid();
		}

		return DataFieldTypeEditor switch {

			TextDataFieldEditor => new() { Value = new TextDataField {
				Name = Name.InputObject
			}},

			SelectionDataFieldEditor selectionDataFieldEditor => new() { Value = new SelectionDataField {
				Name = Name.InputObject,
				OptionNames = selectionDataFieldEditor.Options.Select(x => x.OutputObject.Value).ToReadOnly()
			}},

			IntegerDataFieldEditor integerDataFieldEditor => new IEditorToGameSpecificationResult<DataField>.Success { Value = new IntegerDataField {
				Name = Name.InputObject,
				InitialValue = integerDataFieldEditor.InitialValue.OutputObject.Value,
				MinValue = integerDataFieldEditor.MinValue.OutputObject.Value,
				MaxValue = integerDataFieldEditor.MaxValue.OutputObject.Value
			}},

			_ => throw new UnreachableException()
		};
	}

}