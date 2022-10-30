using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesLibrary;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Math.Numbers;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;
using ValidationError = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using Number = UtilitiesLibrary.Math.Numbers.Number;

namespace GameMakerWpf.Validation.Conversion;



public interface INumberConversionErrorSet {

	public ValidationError RequiresValueError { get; init; }

	public Func<char[], ValidationError> InvalidCharactersErrorGetter { get; init; }

	public Func<string, ValidationError> ValueTooLargeErrorGetter { get; init; }
}

public interface ISignedNumberConversionErrorSet : INumberConversionErrorSet {

	public ValidationError MinusSignMustBeAtStartError { get; init; }
	
	public Func<string, ValidationError> ValueTooNegativeErrorGetter { get; init; }
}

public interface IUnsignedNumberConversionErrorSet : INumberConversionErrorSet {

	public ValidationError CannotBeNegativeError { get; init; }
}

public interface IIntegerConversionErrorSet : INumberConversionErrorSet {

	public ValidationError MustBeIntegerError { get; init; }
}

public interface IFloatConversionErrorSet : INumberConversionErrorSet {

	public ValidationError TooManyDecimalPointsError { get; init; }
}



public class FloatConversionErrorSet : ISignedNumberConversionErrorSet, IFloatConversionErrorSet {

	public required ValidationError RequiresValueError { get; init; }

	public required Func<char[], ValidationError> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, ValidationError> ValueTooLargeErrorGetter { get; init; }

	public required Func<string, ValidationError> ValueTooNegativeErrorGetter { get; init; }

	public required ValidationError TooManyDecimalPointsError { get; init; }

	public required ValidationError MinusSignMustBeAtStartError { get; init; }

}

public class IntegerConversionErrorSet : ISignedNumberConversionErrorSet, IIntegerConversionErrorSet {

	public required ValidationError RequiresValueError { get; init; }

	public required Func<char[], ValidationError> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, ValidationError> ValueTooLargeErrorGetter { get; init; }

	public required Func<string, ValidationError> ValueTooNegativeErrorGetter { get; init; }

	public required ValidationError MinusSignMustBeAtStartError { get; init; }

	public required ValidationError MustBeIntegerError { get; init; }

}

public class WholeConversionErrorSet : IUnsignedNumberConversionErrorSet, IIntegerConversionErrorSet {

	public required ValidationError RequiresValueError { get; init; }

	public required Func<char[], ValidationError> InvalidCharactersErrorGetter { get; init; }

	public required Func<string, ValidationError> ValueTooLargeErrorGetter { get; init; }

	public required ValidationError MustBeIntegerError { get; init; }
	
	public required ValidationError CannotBeNegativeError { get; init; }
}



public static class StringConversion {

	private static (Optional<Number>, ReadOnlyList<ValidationError>) ToFloat(string inputString, FloatConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x) && x != '-' && x != '.').ToArray();

		List<ValidationError> errors = new();

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

		return (number.Optionalize(), ReadOnlyList<ValidationError>.Empty);
	}

	private static (Optional<Integer>, ReadOnlyList<ValidationError>) ToInteger(string inputString, IntegerConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x) && x != '-' && x != '.').ToArray();

		List<ValidationError> errors = new();

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

		return (integer.Optionalize(), ReadOnlyList<ValidationError>.Empty);
	}

	private static (Optional<Whole>, ReadOnlyList<ValidationError>) ToWhole(string inputString, WholeConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x)).ToArray();

		List<ValidationError> errors = new();

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

		return (natural.Optionalize(), ReadOnlyList<ValidationError>.Empty);
	}



	private static (Optional<T>, ReadOnlyList<ValidationError>) ToFloatPrimitive<T>(
		string inputString, Func<Number, Result<T, NumberToPrimitiveError>> parser, FloatConversionErrorSet errorSet) {

		(Optional<Number> option, ReadOnlyList<ValidationError> numberConversionErrors) = ToFloat(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}

		Number number = option.Value;
		Result<T, NumberToPrimitiveError> result = parser.Invoke(number);

		return result.Resolve() switch {

			NumberToPrimitiveError { ErrorType: NumberToPrimitiveError.Types.ValueBelowMin}
				=> (Optional.NoValue, new(errorSet.ValueTooNegativeErrorGetter(inputString))),

			NumberToPrimitiveError { ErrorType: NumberToPrimitiveError.Types.ValueAboveMax}
				=> (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),

			T success => (success.Optionalize(), ReadOnlyList<ValidationError>.Empty),

			_ => throw new ShouldMatchOtherCaseException()
		};
	} 

	private static (Optional<T>, ReadOnlyList<ValidationError>) ToIntegerPrimitive<T>(
		string inputString, Func<Integer, Result<T, IntegerToPrimitiveError>> parser, IntegerConversionErrorSet errorSet) {

		(Optional<Integer> option, ReadOnlyList<ValidationError> numberConversionErrors) = ToInteger(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}

		Integer integer = option.Value;
		Result<T, IntegerToPrimitiveError> result = parser.Invoke(integer);

		return result.Resolve() switch {

			IntegerToPrimitiveError { ErrorType: IntegerToPrimitiveError.Types.ValueBelowMin}
				=> (Optional.NoValue, new(errorSet.ValueTooNegativeErrorGetter(inputString))),

			IntegerToPrimitiveError { ErrorType: IntegerToPrimitiveError.Types.ValueAboveMax}
				=> (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),

			T success => (success.Optionalize(), ReadOnlyList<ValidationError>.Empty),

			_ => throw new ShouldMatchOtherCaseException()
		};
	}

	private static (Optional<T>, ReadOnlyList<ValidationError>) ToWholePrimitive<T>(
		string inputString, Func<Whole, Result<T, IntegerToPrimitiveError>> parser, WholeConversionErrorSet errorSet) {

		(Optional<Whole> option, ReadOnlyList<ValidationError> numberConversionErrors) = ToWhole(inputString, errorSet);

		if (!option.HasValue) {
			return (Optional.NoValue, numberConversionErrors);
		}
		
		Whole whole = option.Value;
		Result<T, IntegerToPrimitiveError> result = parser.Invoke(whole);

		return result.Resolve() switch {

			IntegerToPrimitiveError { ErrorType: IntegerToPrimitiveError.Types.ValueBelowMin}
				=> (Optional.NoValue, new(errorSet.CannotBeNegativeError)),

			IntegerToPrimitiveError { ErrorType: IntegerToPrimitiveError.Types.ValueAboveMax}
				=> (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString))),

			T success => (success.Optionalize(), ReadOnlyList<ValidationError>.Empty),

			_ => throw new ShouldMatchOtherCaseException()
		};
	}



	public static (Optional<byte>, ReadOnlyList<ValidationError>) ToByte(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToByte, errorSet);
	}

	public static (Optional<ushort>, ReadOnlyList<ValidationError>) ToUshort(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUshort, errorSet);
	}

	public static (Optional<uint>, ReadOnlyList<ValidationError>) ToUint(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUint, errorSet);
	}

	public static (Optional<ulong>, ReadOnlyList<ValidationError>) ToUlong(string inputString, WholeConversionErrorSet errorSet) {

		return ToWholePrimitive(inputString, Whole.ToUlong, errorSet);
	}



	public static (Optional<short>, ReadOnlyList<ValidationError>) ToShort(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToShort, errorSet);
	}

	public static (Optional<int>, ReadOnlyList<ValidationError>) ToInt(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToInt, errorSet);
	}

	public static (Optional<long>, ReadOnlyList<ValidationError>) ToLong(string inputString, IntegerConversionErrorSet errorSet) {

		return ToIntegerPrimitive(inputString, Integer.ToLong, errorSet);
	}



	public static (Optional<float>, ReadOnlyList<ValidationError>) ToFloat32(string inputString, FloatConversionErrorSet errorSet) {

		return ToFloatPrimitive(inputString, Number.ToFloat, errorSet);
	}

	public static (Optional<double>, ReadOnlyList<ValidationError>) ToFloat64(string inputString, FloatConversionErrorSet errorSet) {

		return ToFloatPrimitive(inputString, Number.ToDouble, errorSet);
	}

}