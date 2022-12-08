using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CCSSDomain;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Inputs;
using static CCSSDomain.Models.DataField;

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
			SelectionDataFieldEditingData selectionDataField => new SelectionDataFieldEditor(selectionDataField),
			IntegerDataFieldEditingData integerDataField => new IntegerDataFieldEditor(integerDataField),
			_ => throw new ShouldMatchOtherCaseException()
		};

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = DataFieldValidator.NameConverter,
			Inverter = DataFieldValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddValidationRule(DataFieldValidator.NameValidator_Length)
		.AddValidationRule<IEnumerable<DataFieldEditor>>(DataFieldValidator.NameValidator_Uniqueness, () => GameEditor.DataFields, false, GameEditor.DataFieldNameChanged)
		.CreateSingleInput();
	}

	public DataFieldEditingData ToEditingData() {

		return DataFieldTypeEditor switch {

			TextDataFieldEditor => new TextDataFieldEditingData() {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Text
			},

			SelectionDataFieldEditor selectionDataFieldEditor => new SelectionDataFieldEditingData() {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Selection,
				OptionNames = selectionDataFieldEditor.Options.Select(x => x.InputObject).ToReadOnly()
			},

			IntegerDataFieldEditor integerDataFieldEditor => new IntegerDataFieldEditingData() {
				Name = Name.InputObject,
				DataFieldType = DataFieldType.Integer,
				InitialValue = integerDataFieldEditor.InitialValue.InputObject,
				MinValue = integerDataFieldEditor.MinValue.InputObject,
				MaxValue = integerDataFieldEditor.MaxValue.InputObject
			},

			_ => throw new ShouldMatchOtherCaseException()
		};
	}

	private void ChangeDataFieldType(DataFieldType dataFieldType) {

		DataFieldTypeEditor = dataFieldType switch {
			DataFieldType.Text => new TextDataFieldEditor(),
			DataFieldType.Selection => new SelectionDataFieldEditor(DefaultEditingDataValues.DefaultSelectionDataFieldEditingData),
			DataFieldType.Integer => new IntegerDataFieldEditor(DefaultEditingDataValues.DefaultIntegerDataFieldEditingData),
			_ => throw new ShouldMatchOtherCaseException()
		};

		GameEditor.DataFieldTypeChanged.Invoke();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}