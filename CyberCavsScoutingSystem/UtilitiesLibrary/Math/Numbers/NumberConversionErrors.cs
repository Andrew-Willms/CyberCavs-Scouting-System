namespace UtilitiesLibrary.Math.Numbers; 



public class InvalidCharactersError : Error {

	public InvalidCharactersError() { }

	public InvalidCharactersError(string message) : base(message) { }

	public InvalidCharactersError(string message, Error innerError) : base(message, innerError) { }
}

public class ArgumentNullError : Error {

	public ArgumentNullError() { }

	public ArgumentNullError(string message) : base(message) { }

	public ArgumentNullError(string message, Error innerError) : base(message, innerError) { }
}

public class ValueTooLargeError : Error {

	public ValueTooLargeError() { }

	public ValueTooLargeError(string message) : base(message) { }

	public ValueTooLargeError(string message, Error innerError) : base(message, innerError) { }
}

public class ValueTooSmallError : Error {

	public ValueTooSmallError() { }

	public ValueTooSmallError(string message) : base(message) { }

	public ValueTooSmallError(string message, Error innerError) : base(message, innerError) { }
}

public class ValueIsNotWholeNumberError : Error {

	public ValueIsNotWholeNumberError() { }

	public ValueIsNotWholeNumberError(string message) : base(message) { }

	public ValueIsNotWholeNumberError(string message, Error innerError) : base(message, innerError) { }
}

public class ValueIsNotPositiveError : Error {

	public ValueIsNotPositiveError() { }

	public ValueIsNotPositiveError(string message) : base(message) { }

	public ValueIsNotPositiveError(string message, Error innerError) : base(message, innerError) { }
}