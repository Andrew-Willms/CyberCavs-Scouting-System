using System;
using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Results;



public class Result<TError> where TError : Error {

	private Optional<Success> Success { get; }
	private Optional<Error> Error { get; }

	public Result Resolve() {

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
		Error = Optional.Optional.NoValue;
	}

	private Result(Error error) {
		Success = Optional.Optional.NoValue;
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

	public Result Resolve() {

		if (Success.HasValue) {
			return Success.Value;
		}

		if (Error.HasValue) {
			return Error.Value;
		}

		throw new ShouldNotReachException();
	}

	private Result(Success<TValue> success) {
		Success = success.Optionalize();
		Error = Optional.Optional.NoValue;
	}

	private Result(TError error) {
		Success = Optional.Optional.NoValue;
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



public class Result { }



public class Success : Result { }

public class Success<T> : Result {

	public required T Value { get; init; }

}



public class Error : Result {

	public string? Message { get; init; }

	public Optional<Error> InnerError { get; init; } = Optional.Optional.NoValue;

}

public abstract class Error<TErrorEnum> : Error where TErrorEnum : Enum {

	public required TErrorEnum ErrorType { get; init; }

}