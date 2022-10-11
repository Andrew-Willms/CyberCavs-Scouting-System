using System.Windows.Media;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain;



public class AllianceEditingData {

	private GameEditingData EditingData { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }



	public AllianceEditingData(GameEditingData editingData, Alliance initialValues) {

		EditingData = editingData;

		Name = new SingleInputCreator<string, string, ErrorSeverity> {
				Converter = AllianceValidator.NameConverter,
				Inverter = AllianceValidator.NameInverter,
				InitialInput = initialValues.Name
			}.AddOnChangeValidator(AllianceValidator.NameValidator_Length)
			.AddOnChangeValidator(AllianceValidator.NameValidator_EndsWithAlliance)
			.AddTriggeredValidator(AllianceValidator.NameValidator_Uniqueness, () => EditingData.Alliances, EditingData.AllianceNameChanged)
			.CreateSingleInput();

		AllianceColor = new MultiInputCreator<Color, ErrorSeverity, byte, byte, byte> {

				Converter = AllianceValidator.ColorConverter, 
				Inverter = AllianceValidator.ColorInverter,

				InputComponent1 = new SingleInputCreator<byte, string, ErrorSeverity> {
					Converter = AllianceValidator.ColorComponentConverter,
					Inverter = AllianceValidator.ColorComponentInverter,
					InitialInput = initialValues.Color.R.ToString()
				}.CreateSingleInput(),

				InputComponent2 = new SingleInputCreator<byte, string, ErrorSeverity> {
					Converter = AllianceValidator.ColorComponentConverter,
					Inverter = AllianceValidator.ColorComponentInverter,
					InitialInput = initialValues.Color.G.ToString()
				}.CreateSingleInput(),

				InputComponent3 = new SingleInputCreator<byte, string, ErrorSeverity> {
					Converter = AllianceValidator.ColorComponentConverter,
					Inverter = AllianceValidator.ColorComponentInverter,
					InitialInput = initialValues.Color.B.ToString()
				}.CreateSingleInput(),

			}.AddTriggeredValidator(AllianceValidator.ColorCovalidator_Uniqueness, () => EditingData.Alliances, EditingData.AllianceColorChanged)
			.CreateSingleInput();
	}
}
