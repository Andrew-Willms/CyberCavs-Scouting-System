namespace UtilitiesLibrary;



public abstract class Result<T> { }



public class Success<T> : Result<T> {

	public T Value { get; }

	public Success(T value) {
		Value = value;
	}

}



public class Failure<T> : Result<T> {

	public Error Error { get; }

	public Failure(Error error) {
		Error = error;
	}

}



public class Error {

	public string Message { get; } = "";

	public Error? InnerError { get; }

	public Error() { }

	public Error(string message) {
		Message = message;
	}

	public Error(string message, Error innerError) {
		Message = message;
		InnerError = innerError;
	}

}