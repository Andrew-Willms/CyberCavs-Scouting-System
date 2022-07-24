using System;

namespace WPFUtilities.Validation; 



public class NullInputObjectInConverterException : Exception {

	public NullInputObjectInConverterException() { }

	public NullInputObjectInConverterException(string message) : base(message) { }

	public NullInputObjectInConverterException(string message, Exception inner) : base(message, inner) { }

	public static void ThrowIfNull(object? inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverterException();
		}
	}

}

public class NullInputObjectInInverterException : Exception {

	public NullInputObjectInInverterException() { }

	public NullInputObjectInInverterException(string message) : base(message) { }

	public NullInputObjectInInverterException(string message, Exception inner) : base(message, inner) { }

	public static void ThrowIfNull(object? inputString) {

		if (inputString is null) {
			throw new NullInputObjectInInverterException();
		}
	}

}

public class ShouldNotReachException : Exception {

	public ShouldNotReachException() { }

	public ShouldNotReachException(string message) : base(message) { }

	public ShouldNotReachException(string message, Exception inner) : base(message, inner) { }
}

public class ShouldMatchOtherCaseException : Exception {

	public ShouldMatchOtherCaseException() { }

	public ShouldMatchOtherCaseException(string message) : base(message) { }

	public ShouldMatchOtherCaseException(string message, Exception inner) : base(message, inner) { }
}