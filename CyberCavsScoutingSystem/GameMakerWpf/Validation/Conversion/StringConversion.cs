using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesLibrary;
using UtilitiesLibrary.Extensions;
using UtilitiesLibrary.Math.Numbers;
using UtilitiesLibrary.Validation;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using Number = UtilitiesLibrary.Math.Numbers.Number;

namespace GameMakerWpf.Validation.Conversion;



public interface INumberConversionErrorSet {

	public Error RequiresValueError { get; init; }

	public Func<char[], Error> InvalidCharactersErrorGetter { get; init; }

	public Func<string, Error> ValueTooLargeErrorGetter { get; init; }
}

public interface ISignedNumberConversionErrorSet : INumberConversionErrorSet {

	public Error MinusSignMustBeAtStartError { get; init; }

	public Func<string, Error> ValueTooSmallErrorGetter { get; init; }
}

public interface IUnsignedNumberConversionErrorSet : INumberConversionErrorSet {

	public Error CannotBeNegativeError { get; init; }
}

public interface IIntegerConversionErrorSet : INumberConversionErrorSet {

	public Error MustBeIntegerError { get; init; }
}

public interface IFloatConversionErrorSet : INumberConversionErrorSet {

	public Error TooManyDecimalPointsError { get; init; }
}



public class FloatConversionErrorSet : ISignedNumberConversionErrorSet, IFloatConversionErrorSet {

	public required Error RequiresValueError { get; init; }

	public required Func<char[], Error> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, Error> ValueTooLargeErrorGetter { get; init; }

	public required Func<string, Error> ValueTooSmallErrorGetter { get; init; }

	public required Error TooManyDecimalPointsError { get; init; }

	public required Error MinusSignMustBeAtStartError { get; init; }

}

public class IntegerConversionErrorSet : ISignedNumberConversionErrorSet, IIntegerConversionErrorSet {

	public required Error RequiresValueError { get; init; }

	public required Func<char[], Error> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, Error> ValueTooLargeErrorGetter { get; init; }

	public required Func<string, Error> ValueTooSmallErrorGetter { get; init; }

	public required Error MinusSignMustBeAtStartError { get; init; }

	public required Error MustBeIntegerError { get; init; }

}

public class WholeConversionErrorSet : IUnsignedNumberConversionErrorSet, IIntegerConversionErrorSet {

	public required Error RequiresValueError { get; init; }

	public required Func<char[], Error> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, Error> ValueTooLargeErrorGetter { get; init; }

	public required Error MustBeIntegerError { get; init; }
	
	public required Error CannotBeNegativeError { get; init; }
}



public static class StringConversion {
	
	private static (Optional<Number>, ReadOnlyList<Error>) ToFloat(string inputString, FloatConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x) && x != '-' && x != '.').ToArray();

		List<Error> errors = new();

		if (invalidCharacters.Any()) {
			errors.Add(errorSet.InvalidCharactersErrorGetter(invalidCharacters));
		}

		if (inputString.Multiple('-') || (inputString.OnlyOne('-') && !inputString.StartsWith('-'))) {
			errors.Add(errorSet.MinusSignMustBeAtStartError);
		}

		if (inputString.Multiple('.')) {
			errors.Add(errorSet.TooManyDecimalPointsError);
		}

		if (errors.Any()) {
			return (Optional.NoValue, errors.ToReadOnly());
		}

		Number? number = Number.Parse(inputString);

		if (number is null) {
			throw new InvalidOperationException($"Somehow parsing the input as a {typeof(Number)} failed.");
		}

		return (Optional<Number>.FromValue(number.Value), ReadOnlyList<Error>.Empty);
	}

	private static (Optional<Integer>, ReadOnlyList<Error>) ToInteger(string inputString, IntegerConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x) && x != '-' && x != '.').ToArray();

		List<Error> errors = new();

		if (invalidCharacters.Any()) {
			errors.Add(errorSet.InvalidCharactersErrorGetter(invalidCharacters));
		}

		if (inputString.Multiple('-') || (inputString.OnlyOne('-') && !inputString.StartsWith('-'))) {
			errors.Add(errorSet.MinusSignMustBeAtStartError);
		}

		if (inputString.Contains('.')) {
			errors.Add(errorSet.MustBeIntegerError);
		}

		if (errors.Any()) {
			return (Optional.NoValue, errors.ToReadOnly());
		}

		Integer? integer = Integer.Parse(inputString);

		if (integer is null) {
			throw new InvalidOperationException($"Somehow parsing the input as a {typeof(Integer)} failed.");
		}

		return (Optional<Integer>.FromValue(integer), ReadOnlyList<Error>.Empty);
	}

	private static (Optional<Whole>, ReadOnlyList<Error>) ToWhole(string inputString, WholeConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x)).ToArray();

		List<Error> errors = new();

		if (invalidCharacters.Any()) {
			errors.Add(errorSet.InvalidCharactersErrorGetter(invalidCharacters));
		}

		if (inputString.Contains('-')) {
			errors.Add(errorSet.CannotBeNegativeError);
		}

		if (inputString.Contains('.')) {
			errors.Add(errorSet.MustBeIntegerError);
		}

		if (errors.Any()) {
			return (Optional.NoValue, errors.ToReadOnly());
		}

		Whole? natural = Whole.Parse(inputString);

		if (natural is null) {
			throw new InvalidOperationException($"Somehow parsing the input as a {typeof(Whole)} failed.");
		}

		return (Optional<Whole>.FromValue(natural), ReadOnlyList<Error>.Empty);
	}



	private static (Optional<T>, ReadOnlyList<Error>) ToFloatPrimitive<T>(string inputString, Func<Number, Result<T>> parser,
		FloatConversionErrorSet errorSet) {

		(Optional<Number> option, ReadOnlyList<Error> numberConversionErrors) = ToFloat(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}

		Number number = option.Value;
		Result<T> result = parser.Invoke(number);

		return result switch {
			Failure<T> {Error: ValueTooLargeError} => (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),
			Failure<T> {Error: ValueTooSmallError} => (Optional.NoValue, new(errorSet.ValueTooSmallErrorGetter(inputString))),
			Success<T> success => (Optional<T>.FromValue(success.Value), ReadOnlyList<Error>.Empty),
			_ => throw new ShouldMatchOtherCaseException()
		};
	} 

	private static (Optional<T>, ReadOnlyList<Error>) ToIntegerPrimitive<T>(string inputString, Func<Integer, Result<T>> parser,
		IntegerConversionErrorSet errorSet) {

		(Optional<Integer> option, ReadOnlyList<Error> numberConversionErrors) = ToInteger(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}

		Integer integer = option.Value;
		Result<T> result = parser.Invoke(integer);

		return result switch {
			Failure<T> {Error: ValueTooLargeError} => (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),
			Failure<T> {Error: ValueTooSmallError} => (Optional.NoValue, new(errorSet.ValueTooSmallErrorGetter(inputString))),
			Success<T> success => (Optional<T>.FromValue(success.Value), ReadOnlyList<Error>.Empty),
			_ => throw new ShouldMatchOtherCaseException()
		};
	}

	private static (Optional<T>, ReadOnlyList<Error>) ToWholePrimitive<T>(string inputString, Func<Whole, Result<T>> parser,
		WholeConversionErrorSet errorSet) {

		(Optional<Whole> option, ReadOnlyList<Error> numberConversionErrors) = ToWhole(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}

		Whole whole = option.Value;
		Result<T> result = parser.Invoke(whole);

		return result switch {
			Failure<T> {Error: ValueTooLargeError} => (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),
			Failure<T> {Error: ValueIsNotPositiveError} => (Optional.NoValue, new(errorSet.CannotBeNegativeError)),
			Success<T> success => (Optional<T>.FromValue(success.Value), ReadOnlyList<Error>.Empty),
			_ => throw new ShouldMatchOtherCaseException()
		};
	}




	public static (Optional<byte>, ReadOnlyList<Error>) ToByte(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToByte, errorSet);
	}

	public static (Optional<ushort>, ReadOnlyList<Error>) ToUshort(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUshort, errorSet);
	}

	public static (Optional<uint>, ReadOnlyList<Error>) ToUint(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUint, errorSet);
	}

	public static (Optional<ulong>, ReadOnlyList<Error>) ToUlong(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUlong, errorSet);
	}



	public static (Optional<short>, ReadOnlyList<Error>) ToShort(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToShort, errorSet);
	}

	public static (Optional<int>, ReadOnlyList<Error>) ToInt(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToInt, errorSet);
	}

	public static (Optional<long>, ReadOnlyList<Error>) ToLong(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToLong, errorSet);
	}



	public static (Optional<float>, ReadOnlyList<Error>) ToFloat32(string inputString, FloatConversionErrorSet errorSet) {

		return ToFloatPrimitive(inputString, Number.ToFloat, errorSet);
	}

	public static (Optional<double>, ReadOnlyList<Error>) ToDouble(string inputString, FloatConversionErrorSet errorSet) {

		return ToFloatPrimitive(inputString, Number.ToDouble, errorSet);
	}

}