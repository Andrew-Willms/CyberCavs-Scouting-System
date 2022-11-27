using System.Collections.Generic;
using System.Linq;
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
		}.AddValidationRule(AllianceValidator.NameValidator_Length)
		.AddValidationRule(AllianceValidator.NameValidator_EndsWithAlliance)
		.AddValidationRule(AllianceValidator.NameValidator_Uniqueness, () => GameEditor.Alliances, false, GameEditor.AllianceNameChanged)
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
		}.AddValidationRule<IEnumerable<AllianceEditor>>(AllianceValidator.ColorCovalidator_Uniqueness,
				() => GameEditor.Alliances.Where(x => x != this), false, GameEditor.AllianceColorChanged)
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