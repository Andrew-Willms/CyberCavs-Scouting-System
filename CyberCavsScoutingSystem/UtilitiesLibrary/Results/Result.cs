using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Results;



public abstract class Success { }

public abstract class Error {

	public Optional<string> Message { get; init; } = Optional.Optional.NoValue;

	public Optional<Error> InnerError { get; init; } = Optional.Optional.NoValue;

}


public interface IResult {

	public class Success : Results.Success, IResult { }

	public class Error : Results.Error, IResult { }

}

public interface IResult<T> {

	public class Success : Results.Success, IResult<T> {

		public required T Value { get; init; }

	}

	public class Error : Results.Error, IResult<T> { }

}