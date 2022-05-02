namespace WPFUtilities; 



public abstract class ValidationErrorSeverityEnum<T> : OrderedSmartEnum<T> {

	public bool IsFatal { get; }

	protected ValidationErrorSeverityEnum(string name, int value, bool isFatal) : base(name, value) {

		IsFatal = isFatal;
	}

}

public interface IValidationErrorSeverityEnum<out T> where T : ValidationErrorSeverityEnum<T> {

	public static abstract T NoError { get; }

}