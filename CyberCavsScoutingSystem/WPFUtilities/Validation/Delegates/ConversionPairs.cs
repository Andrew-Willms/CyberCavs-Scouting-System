using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public record ConversionPair<TOutput, TInput, TSeverityEnum>(
	SingleInputConverterErrorList<TOutput?, TInput, TSeverityEnum> Converter,
	SingleInputInverterErrorList<TOutput, TInput?, TSeverityEnum> Inverter)

	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
	
	public ConversionPair(SingleInputConverterSingleError<TOutput?, TInput, TSeverityEnum> converter,
		SingleInputInverterErrorList<TOutput, TInput?, TSeverityEnum> inverter)
		: this(DelegateConverters.SingleToErrorListConvert(converter), inverter) { }

	public ConversionPair(SingleInputConverterErrorList<TOutput?, TInput, TSeverityEnum> converter,
		SingleInputInverterSingleError<TOutput, TInput?, TSeverityEnum> inverter)
		: this(converter, DelegateConverters.SingleToErrorListInvert(inverter)) { }

	public ConversionPair(SingleInputConverterSingleError<TOutput?, TInput, TSeverityEnum> converter,
		SingleInputInverterSingleError<TOutput, TInput?, TSeverityEnum> inverter)
		: this(DelegateConverters.SingleToErrorListConvert(converter),
			DelegateConverters.SingleToErrorListInvert(inverter)) { }

}