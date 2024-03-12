using System.Diagnostics.CodeAnalysis;
using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Results;



public abstract class Success;

public abstract class Error {

	public string Message { get; init; } = string.Empty;

	public Optional<Error> InnerError { get; init; } = Optional.Optional.NoValue;

	public Error() { }

	public Error(string message) {
		Message = message;
	}

	public Error(Error innerError) {
		InnerError = innerError.Optionalize();
	}

	public Error(string message, Error innerError) {
		Message = message;
		InnerError = innerError.Optionalize();
	}

}



public class Test {

	public void TestTest() {

		IResult<int> test1 = ResultFunction();

		IResult test2 = ResultFunction();

	}

	private static IResult<int> ResultFunction() {

		return null!;
	}

}

public interface IResult {

	public class Success : Results.Success, IResult;

	public class Error : Results.Error, IResult;

}

public interface IResult<T> {

	public class Success : Results.Success, IResult<T> {

		public required T Value { get; init; }

		public static implicit operator Success(T value) {
			return new() { Value = value };
		}

		public Success() { }

		[SetsRequiredMembers]
		public Success(T value) {
			Value = value;
		}

	}

	public class Error : Results.Error, IResult<T> {

		public Error() { }

		public Error(string message) : base(message) { }

		public Error(Results.Error innerError) : base(innerError) { }

		public Error(string message, Results.Error innerError) : base(message, innerError) { }

	}

}