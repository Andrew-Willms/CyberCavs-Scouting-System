using System;

namespace UtilitiesLibrary.Validation;



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