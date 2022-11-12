using System.Windows.Media;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors;



public class AllianceEditor {

	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }
	public SingleInput<byte, string, ErrorSeverity> RedColorValue { get; }
	public SingleInput<byte, string, ErrorSeverity> GreenColorValue { get; }
	public SingleInput<byte, string, ErrorSeverity> BlueColorValue { get; }



	public AllianceEditor(GameEditor gameEditor, AllianceEditingData initialValues) {

		GameEditor = gameEditor;

		Name = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = AllianceValidator.NameConverter,
			Inverter = AllianceValidator.NameInverter,
			InitialInput = initialValues.Name
		}.AddOnChangeValidator(AllianceValidator.NameValidator_Length)
		.AddOnChangeValidator(AllianceValidator.NameValidator_EndsWithAlliance)
		.AddTriggeredValidator(AllianceValidator.NameValidator_Uniqueness, () => GameEditor.Alliances, GameEditor.AllianceNameChanged)
		.CreateSingleInput();

		RedColorValue = new SingleInputCreator<byte, string, ErrorSeverity> {
			Converter = AllianceValidator.ColorComponentConverter,
			Inverter = AllianceValidator.ColorComponentInverter,
			InitialInput = initialValues.RedColorValue
		}.CreateSingleInput();

		GreenColorValue = new SingleInputCreator<byte, string, ErrorSeverity> {
			Converter = AllianceValidator.ColorComponentConverter,
			Inverter = AllianceValidator.ColorComponentInverter,
			InitialInput = initialValues.GreenColorValue
		}.CreateSingleInput();

		BlueColorValue = new SingleInputCreator<byte, string, ErrorSeverity> {
			Converter = AllianceValidator.ColorComponentConverter,
			Inverter = AllianceValidator.ColorComponentInverter,
			InitialInput = initialValues.BlueColorValue
		}.CreateSingleInput();

		AllianceColor = new MultiInputCreator<Color, ErrorSeverity, byte, byte, byte> { 
			Converter = AllianceValidator.ColorConverter, 
			Inverter = AllianceValidator.ColorInverter,
			InputComponent1 = RedColorValue,
			InputComponent2 = GreenColorValue,
			InputComponent3 = BlueColorValue
		}.AddTriggeredValidator(AllianceValidator.ColorCovalidator_Uniqueness, () => GameEditor.Alliances, GameEditor.AllianceColorChanged)
		.CreateMultiInput();
	}

	public AllianceEditingData ToEditingData() {

		return new() {
			Name = Name.InputObject,
			RedColorValue = RedColorValue.InputObject,
			GreenColorValue = GreenColorValue.InputObject,
			BlueColorValue = BlueColorValue.InputObject
		};
	}

}