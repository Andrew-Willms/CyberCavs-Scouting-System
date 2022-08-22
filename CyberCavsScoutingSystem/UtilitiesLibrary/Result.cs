namespace UtilitiesLibrary;



public abstract class Result<T> {

	public bool WasSuccessful { get; }

	protected Result(bool wasSuccessful) {
		WasSuccessful = wasSuccessful;
	}

}



public class Success<T> : Result<T> {

	public T Value { get; }

	public Success(T value) : base(true) {
		Value = value;
	}
}

public class Failure<T> : Result<T> {

	public Error Error { get; }

	public Failure(Error error) : base(false) {
		Error = error;
	}

}



public class Error {

	public string Message { get; }

	public Error? InnerError { get; }

	public Error() {
		Message = "";
	}

	public Error(string message) {
		Message = message;
	}

	public Error(string message, Error innerError) {
		Message = message;
		InnerError = innerError;
	}

}