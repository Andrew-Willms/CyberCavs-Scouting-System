using System.Collections.ObjectModel;
using System.ComponentModel;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Domain.DomainData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using static CCSSDomain.Models.DataField;

namespace GameMakerWpf.Domain;



public class GeneralDataFieldEditingData : INotifyPropertyChanged {

	private GameEditingData EditingData { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public DataFieldTypeEditingData DataFieldTypeEditingData { get; private set; }

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
			OnPropertyChanged(nameof(DataFieldTypeEditingData));
		}
	}



	public GeneralDataFieldEditingData(GameEditingData editingData, DataField initialValues) {

		EditingData = editingData;

		DataFieldType = initialValues switch {
			TextDataField => DataFieldType.Text,
			SelectionDataField => DataFieldType.Selection,
			IntegerDataField => DataFieldType.Integer,
			_ => throw new ShouldMatchOtherCaseException()
		};

		DataFieldTypeEditingData = initialValues switch {
			TextDataField => new TextDataFieldEditingData(),
			SelectionDataField selectionDataField => new SelectionDataFieldEditingData(selectionDataField),
			IntegerDataField integerDataField => new IntegerDataFieldEditingData(integerDataField),
			_ => throw new ShouldMatchOtherCaseException()
		};

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
			Converter = DataFieldValidator.NameConverter,
			Inverter = DataFieldValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddOnChangeValidator(DataFieldValidator.NameValidator_Length)
		.AddTriggeredValidator(DataFieldValidator.NameValidator_Uniqueness, () => EditingData.DataFields, EditingData.DataFieldNameChanged)
		.CreateSingleInput();
	}

	private void ChangeDataFieldType(DataFieldType dataFieldType) {

		DataFieldTypeEditingData = dataFieldType switch {
			DataFieldType.Text => new TextDataFieldEditingData(),
			DataFieldType.Selection => new SelectionDataFieldEditingData(DefaultEditingDataValues.DefaultSelectionDataField),
			DataFieldType.Integer => new IntegerDataFieldEditingData(DefaultEditingDataValues.DefaultIntegerDataField),
			_ => throw new ShouldMatchOtherCaseException()
		};

		EditingData.DataFieldTypeChanged.Invoke();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}




public abstract class DataFieldTypeEditingData { }

public class TextDataFieldEditingData : DataFieldTypeEditingData { }



public class SelectionDataFieldEditingData : DataFieldTypeEditingData {

	private ObservableCollection<SingleInput<string, string, ErrorSeverity>> _Options { get; } = new();
	public ReadOnlyObservableCollection<SingleInput<string, string, ErrorSeverity>> Options => new(_Options);

	public ValidationEvent OptionNameChanged { get; } = new();

	public SelectionDataFieldEditingData(SelectionDataField initialValues) {

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



public class IntegerDataFieldEditingData : DataFieldTypeEditingData {

	public SingleInput<int, string, ErrorSeverity> InitialValue { get; }

	public SingleInput<int, string, ErrorSeverity> MinValue { get; }

	public SingleInput<int, string, ErrorSeverity> MaxValue { get; }



	public IntegerDataFieldEditingData(IntegerDataField initialValues) {

		InitialValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();

		MinValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();

		MaxValue = new SingleInputCreator<int, string, ErrorSeverity> {
				Converter = IntegerDataFieldValidator.IntegerValueConverter,
				Inverter = IntegerDataFieldValidator.IntegerValueInverter,
				InitialInput = initialValues.InitialValue.ToString()
			}.CreateSingleInput();
	}

}