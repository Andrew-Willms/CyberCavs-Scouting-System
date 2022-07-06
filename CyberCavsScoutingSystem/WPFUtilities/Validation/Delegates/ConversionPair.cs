using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public record ConversionPair<TOutput, TInput, TSeverityEnum>(
	InputConverterErrorList<TOutput, TInput, TSeverityEnum> Converter,
	InputInverterErrorList<TOutput, TInput, TSeverityEnum> Inverter)

	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
	
	public ConversionPair(InputConverterSingleError<TOutput, TInput, TSeverityEnum> converter,
		InputInverterErrorList<TOutput, TInput, TSeverityEnum> inverter)
		: this(DelegateConverters.SingleToErrorListConvert(converter), inverter) { }

	public ConversionPair(InputConverterErrorList<TOutput, TInput, TSeverityEnum> converter,
		InputInverterSingleError<TOutput, TInput, TSeverityEnum> inverter)
		: this(converter, DelegateConverters.SingleToErrorListInvert(inverter)) { }

	public ConversionPair(InputConverterSingleError<TOutput, TInput, TSeverityEnum> converter,
		InputInverterSingleError<TOutput, TInput, TSeverityEnum> inverter)
		: this(DelegateConverters.SingleToErrorListConvert(converter),
			DelegateConverters.SingleToErrorListInvert(inverter)) { }

}