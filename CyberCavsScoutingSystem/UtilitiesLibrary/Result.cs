using System;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;

namespace UtilitiesLibrary;



public class Result<TError> where TError : Error {

	private Optional<Success> Success { get; }
	private Optional<Error> Error { get; }

	public object Resolve() {

		if (Success.HasValue) {
			return Success.Value;
		}

		if (Error.HasValue) {
			return Error.Value;
		}

		throw new ShouldNotReachException();
	}

	private Result(Success success) {
		Success = success.Optionalize();
		Error = Optional.NoValue;
	}

	private Result(Error error) {
		Success = Optional.NoValue;
		Error = error.Optionalize();
	}

	public static implicit operator Result<TError>(Success success) {
		return new(success);
	}

	public static implicit operator Result<TError>(TError error) {
		return new(error);
	}

}



public class Result<TValue, TError> where TError : Error {

	private Optional<Success<TValue>> Success { get; }
	private Optional<TError> Error { get; }

	public object Resolve() {

		if (Success.HasValue) {
			return Success.Value.Value!;
		}

		if (Error.HasValue) {
			return Error.Value;
		}

		throw new ShouldNotReachException();
	}

	private Result(Success<TValue> success) {
		Success = success.Optionalize();
		Error = Optional.NoValue;
	}

	private Result(TError error) {
		Success = Optional.NoValue;
		Error = error.Optionalize();
	}

	public static implicit operator Result<TValue, TError>(TValue value) {
		return new(new Success<TValue> { Value = value });
	}

	public static implicit operator Result<TValue, TError>(Success<TValue> success) {
		return new(success);
	}

	public static implicit operator Result<TValue, TError>(TError error) {
		return new(error);
	}

}



public class Success { }

public class Success<T>  {

	public required T Value { get; init; }
}



public class Error {

	public string Message { get; init; } = "";

	public Optional<Error> InnerError { get; init; } = Optional.NoValue;
}

public abstract class Error<TErrorEnum> : Error where TErrorEnum : Enum {

	public required TErrorEnum ErrorType { get; init; }
}

//public class MultiError<TErrorEnum> : Error where TErrorEnum : Enum {
//
//	public required ReadOnlyList<Error<TErrorEnum>> InternalErrors { get; init; }
//}