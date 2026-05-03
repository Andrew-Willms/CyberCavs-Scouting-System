using System.Numerics;
using UtilitiesLibrary.Results;

namespace UtilitiesLibrary.Math.Numbers;



public class InvalidCharactersError : Error { }

public class ArgumentNullError : Error { }

public class ValueTooLargeError : Error { }

public class ValueTooSmallError : Error { }

public class ValueIsNotWholeNumberError : Error { }

public class ValueIsNotPositiveError : Error { }



public interface IIntegerToPrimitiveResult<T> : IResult<T> where T : INumber<T> {

	public class Success : IResult<T>.Success, IIntegerToPrimitiveResult<T> { }

	public class ValueBelowMin : Error, IIntegerToPrimitiveResult<T> { }

	public class ValueAboveMax : Error, IIntegerToPrimitiveResult<T> { }

}

public interface INumberToPrimitiveResult<T> : IResult<T> where T : INumber<T> {

	public class Success : IResult<T>.Success, INumberToPrimitiveResult<T> { }

	public class ValueBelowMin : Error, INumberToPrimitiveResult<T> { }

	public class ValueAboveMax : Error, INumberToPrimitiveResult<T> { }

	//public class DecimalsCannotBeRepresented : IResult<T>.Error, IIntegerToPrimitiveResult<T> { }

}