using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors; 



public class ButtonEditor {
	
	private GameEditor GameEditor { get; }

	public SingleInput<string, string, ErrorSeverity> DataFieldName { get; }

	public SingleInput<string, string, ErrorSeverity> ButtonText { get; }

	public SingleInput<int, string, ErrorSeverity> IncrementAmount { get; }

	public SingleInput<double, string, ErrorSeverity> XPosition { get; }

	public SingleInput<double, string, ErrorSeverity> YPosition { get; }

	public SingleInput<double, string, ErrorSeverity> Width { get; }

	public SingleInput<double, string, ErrorSeverity> Height { get; }

	public ButtonEditor(GameEditor gameEditor, ButtonEditingData initialValues) {

		GameEditor = gameEditor;

		DataFieldName = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = ButtonValidators.DataFieldNameConverter,
			Inverter = ButtonValidators.DataFieldNameInverter,
			InitialInput = initialValues.DataFieldName
		}.AddParameteredOnChangeValidator(ButtonValidators.DataFieldNameValidator_DataFieldOfNameExists, () => GameEditor.DataFields)
		.AddTriggeredValidator(ButtonValidators.DataFieldNameValidator_DataFieldOfNameExists,
			() => GameEditor.DataFields, GameEditor.DataFieldNameChanged, GameEditor.DataFieldTypeChanged)
		.CreateSingleInput();

		ButtonText = new SingleInputCreator<string, string, ErrorSeverity> { 
			Converter = ButtonValidators.ButtonTextConverter,
			Inverter = ButtonValidators.ButtonTextInverter,
			InitialInput = initialValues.ButtonText
		}.AddOnChangeValidator(ButtonValidators.ButtonTextValidator_Length)
		.CreateSingleInput();

		IncrementAmount = new SingleInputCreator<int, string, ErrorSeverity> {
			Converter = ButtonValidators.IncrementAmountConverter,
			Inverter = ButtonValidators.IncrementAmountInverter,
			InitialInput = initialValues.IncrementAmount
		}.CreateSingleInput();

		XPosition = new SingleInputCreator<double, string, ErrorSeverity> {
			Converter = ButtonValidators.PositionConverter,
			Inverter = ButtonValidators.PositionInverter,
			InitialInput = initialValues.IncrementAmount
		}.AddOnChangeValidator(ButtonValidators.PositionValidator_BetweenZeroAndOne)
		.CreateSingleInput();

		YPosition = new SingleInputCreator<double, string, ErrorSeverity> {
			Converter = ButtonValidators.PositionConverter,
			Inverter = ButtonValidators.PositionInverter,
			InitialInput = initialValues.IncrementAmount
		}.AddOnChangeValidator(ButtonValidators.PositionValidator_BetweenZeroAndOne)
		.CreateSingleInput();

		Width = new SingleInputCreator<double, string, ErrorSeverity> {
			Converter = ButtonValidators.PositionConverter,
			Inverter = ButtonValidators.PositionInverter,
			InitialInput = initialValues.IncrementAmount
		}.AddOnChangeValidator(ButtonValidators.PositionValidator_BetweenZeroAndOne)
		.CreateSingleInput();

		Height = new SingleInputCreator<double, string, ErrorSeverity> {
			Converter = ButtonValidators.PositionConverter,
			Inverter = ButtonValidators.PositionInverter,
			InitialInput = initialValues.IncrementAmount
		}.AddOnChangeValidator(ButtonValidators.PositionValidator_BetweenZeroAndOne)
		.CreateSingleInput();
	}

	public ButtonEditingData ToEditingData() {

		return new() {
			DataFieldName = DataFieldName.InputObject,
			ButtonText = ButtonText.InputObject,
			IncrementAmount = IncrementAmount.InputObject,
			XPosition = XPosition.InputObject,
			YPosition = YPosition.InputObject,
			Width = Width.InputObject,
			Height = Height.InputObject,
		};
	}

}