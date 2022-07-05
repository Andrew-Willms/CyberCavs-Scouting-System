using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public record ConversionPair<TOutput, TInput, TSeverityEnum>(
	SingleInputConverter<TOutput?, TInput, TSeverityEnum> Converter,
	SingleInputInverter<TOutput, TInput?, TSeverityEnum> Inverter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;