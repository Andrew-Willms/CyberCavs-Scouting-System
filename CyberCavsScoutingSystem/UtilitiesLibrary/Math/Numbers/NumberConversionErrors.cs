using UtilitiesLibrary.Results;

namespace UtilitiesLibrary.Math.Numbers;



public class InvalidCharactersError : Error { }

public class ArgumentNullError : Error { }

public class ValueTooLargeError : Error { }

public class ValueTooSmallError : Error { }

public class ValueIsNotWholeNumberError : Error { }

public class ValueIsNotPositiveError : Error { }



public class IntegerToPrimitiveError : Error<IntegerToPrimitiveError.Types> {

	public enum Types {
		ValueBelowMin,
		ValueAboveMax
	}

}

public class NumberToPrimitiveError : Error<NumberToPrimitiveError.Types> {

	public enum Types {
		ValueBelowMin,
		ValueAboveMax,
		//DecimalsCannotBeRepresented
	}

}