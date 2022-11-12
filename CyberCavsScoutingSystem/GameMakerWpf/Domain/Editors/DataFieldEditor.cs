using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CCSSDomain;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using static CCSSDomain.Models.DataField;

namespace GameMakerWpf.Domain.Editors;



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
		}.AddOnChangeValidator(DataFieldValidator.NameValidator_Length)
		.AddTriggeredValidator(DataFieldValidator.NameValidator_Uniqueness, () => GameEditor.DataFields, GameEditor.DataFieldNameChanged)
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




public abstract class DataFieldTypeEditor { }



public class TextDataFieldEditor : DataFieldTypeEditor { }



public class SelectionDataFieldEditor : DataFieldTypeEditor {

	private ObservableCollection<SingleInput<string, string, ErrorSeverity>> _Options { get; } = new();
	public ReadOnlyObservableCollection<SingleInput<string, string, ErrorSeverity>> Options => new(_Options);

	public ValidationEvent OptionNameChanged { get; } = new();

	public SelectionDataFieldEditor(SelectionDataFieldEditingData initialValues) {

		foreach (string optionName in initialValues.OptionNames) {

			_Options.Add(new SingleInputCreator<string, string, ErrorSeverity> { 
					Converter = SelectionDataFieldValidator.OptionNameConverter,
					Inverter = SelectionDataFieldValidator.OptionNameInverter,
					InitialInput = optionName
				}.AddOnChangeValidator(SelectionDataFieldValidator.OptionNameValidator_Length)
				.AddTriggeredValidator(SelectionDataFieldValidator.OptionNameValidator_Uniqueness, () => Options, OptionNameChanged)
				.CreateSingleInput()
			);
		}
	}

}



public class IntegerDataFieldEditor : DataFieldTypeEditor {

	public SingleInput<int, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<int, string, ErrorSeverity> MinValue { get; }

	public SingleInput<int, string, ErrorSeverity> MaxValue { get; }



	public IntegerDataFieldEditor(IntegerDataFieldEditingData initialValues) {

		InitialValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();

		MinValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();

		MaxValue = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = IntegerDataFieldValidator.IntegerValueConverter,
			Inverter = IntegerDataFieldValidator.IntegerValueInverter,
			InitialInput = initialValues.InitialValue
		}.CreateSingleInput();
	}

}