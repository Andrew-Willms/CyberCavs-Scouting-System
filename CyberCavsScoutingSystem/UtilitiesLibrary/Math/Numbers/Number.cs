using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using UtilitiesLibrary.Validation;

namespace UtilitiesLibrary.Math.Numbers;



public class Number : IEquatable<Number>, IComparable<Number> {

	public bool IsNegative { get; }

	private int DecimalPosition { get; }

	private ReadOnlyCollection<Digit> Digits { get; }

	public bool IsInteger => DecimalPosition == 0;

	private int LargestDecimalPosition => Digits.Count - DecimalPosition;
	private int SmallestDecimalPosition => -DecimalPosition;



	private Number(bool isNegative, int decimalPosition, ReadOnlyCollection<Digit> digits) {
		IsNegative = isNegative;
		DecimalPosition = decimalPosition;
		Digits = digits;
	}



	public override string ToString() {
		throw new NotImplementedException();
	}



	private Digit GetDigitInPosition(int position) {
		return Digits[position + DecimalPosition];
	}



	public static implicit operator Number(byte value) {

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(false, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(ushort value) {

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(false, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(uint value) {

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(false, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(ulong value) {

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(false, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(short value) {

		bool isNegative = value < 0;

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(int value) {

		bool isNegative = value < 0;

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(long value) {

		bool isNegative = value < 0;

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, 0, digits.AsReadOnly());
	}

	public static implicit operator Number(float value) {

		bool isNegative = value < 0;

		int placesRightOfDecimalPoint = 0;
		while (System.Math.Abs(value % 1 - value) > 1e-10) {
			placesRightOfDecimalPoint++;
			value *= 10;
		}

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
	}

	public static implicit operator Number(double value) {

		bool isNegative = value < 0;

		int placesRightOfDecimalPoint = 0;
		while (System.Math.Abs(value % 1 - value) > 1e-10) {
			placesRightOfDecimalPoint++;
			value *= 10;
		}

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
	}

	public static implicit operator Number(decimal value) {

		bool isNegative = value < 0;

		int placesRightOfDecimalPoint = 0;
		while (System.Math.Abs(value % 1 - value) > 1e-10M) {
			placesRightOfDecimalPoint++;
			value *= 10;
		}

		List<Digit> digits = new();

		while (value > 0) {
			digits.Add(Digit.GetOnesColumn(value));
			value /= 10;
		}

		return new(isNegative, placesRightOfDecimalPoint, digits.AsReadOnly());
	}



	private Result<T, NumberToPrimitiveError> ToNumberPrimitive<T>(
		Number typeMinValue, Number typeMaxValue, Func<Digit, T> digitToT, Func<int, T> tenToThe) where T : INumber<T> {

		if (this < typeMinValue) {
			return new NumberToPrimitiveError { ErrorType = NumberToPrimitiveError.Types.ValueBelowMin };
		}

		if (this > typeMaxValue) {
			return new NumberToPrimitiveError { ErrorType = NumberToPrimitiveError.Types.ValueAboveMax };
		}

		T value = T.Zero;
		for (int position = SmallestDecimalPosition; position < LargestDecimalPosition; position++) {
			value += digitToT(Digits[position]) * tenToThe(position);
		}

		if (IsNegative) {
			value *= Constants.Numbers<T>.MinusOne;
		}

		return value;
	}

	public static Result<byte, NumberToPrimitiveError> ToByte(Number number) {

		return number.ToNumberPrimitive(byte.MinValue, byte.MaxValue, Digit.ToINumber<byte>, i => (byte)System.Math.Pow(10, i));
	}

	public static Result<ushort, NumberToPrimitiveError> ToUshort(Number number) {

		return number.ToNumberPrimitive(ushort.MinValue, ushort.MaxValue, Digit.ToINumber<ushort>, i => (ushort)System.Math.Pow(10, i));
	}

	public static Result<uint, NumberToPrimitiveError> ToUint(Number number) {

		return number.ToNumberPrimitive(uint.MinValue, uint.MaxValue, Digit.ToINumber<uint>, i => (uint)System.Math.Pow(10, i));
	}

	public static Result<ulong, NumberToPrimitiveError> ToUlong(Number number) {

		return number.ToNumberPrimitive(ulong.MinValue, ulong.MaxValue, Digit.ToINumber<ulong>, i => (ulong)System.Math.Pow(10, i));
	}

	public static Result<short, NumberToPrimitiveError> ToShort(Number number) {

		return number.ToNumberPrimitive(short.MinValue, short.MaxValue, Digit.ToINumber<short>, i => (short)System.Math.Pow(10, i));
	}

	public static Result<int, NumberToPrimitiveError> ToInt(Number number) {

		return number.ToNumberPrimitive(int.MinValue, int.MaxValue, Digit.ToINumber<int>, i => (int)System.Math.Pow(10, i));
	}

	public static Result<long, NumberToPrimitiveError> ToLong(Number number) {

		return number.ToNumberPrimitive(long.MinValue, long.MaxValue, Digit.ToINumber<long>, i => (long)System.Math.Pow(10, i));
	}

	public static Result<float, NumberToPrimitiveError> ToFloat(Number number) {

		return number.ToNumberPrimitive(float.MinValue, float.MaxValue, Digit.ToINumber<float>, i => (float)System.Math.Pow(10, i));
	}

	public static Result<double, NumberToPrimitiveError> ToDouble(Number number) {

		return number.ToNumberPrimitive(double.MinValue, double.MaxValue, Digit.ToINumber<double>, i => System.Math.Pow(10, i));
	}



	public static Number Parse(string text) {
		throw new NotImplementedException();
	}



	public override int GetHashCode() {
		return HashCode.Combine(IsNegative, DecimalPosition, Digits);
	}

	public override bool Equals(object? obj) {
		return obj is Number other && Equals(other);
	}

	public bool Equals(Number? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null &&
		       IsNegative == other.IsNegative &&
		       DecimalPosition == other.DecimalPosition &&
		       Digits.SequenceEqual(other.Digits);
	}

	public int CompareTo(Number? other) {

		if (other is null) {
			throw new ArgumentNullException(nameof(other));
		}

		if (Equals(other)) {
			return 0;
		}

		if (IsNegative && !other.IsNegative) {
			return -1;
		}

		if (!IsNegative && other.IsNegative) {
			return 1;
		}

		if (LargestDecimalPosition > other.LargestDecimalPosition) {
			return 1;
		}

		if (LargestDecimalPosition < other.LargestDecimalPosition) {
			return -1;
		}

		int smallestDecimalPosition = System.Math.Min(SmallestDecimalPosition, other.SmallestDecimalPosition);
		for (int position = LargestDecimalPosition; position > smallestDecimalPosition; position--) {
			if (GetDigitInPosition(position) > other.GetDigitInPosition(position)) {
				return 1;
			}

			if (GetDigitInPosition(position) < other.GetDigitInPosition(position)) {
				return -1;
			}
		}

		throw new ShouldNotReachException(
			"Since Equality is short-circuit it should not be possible that the two values are the same and execution should not reach this point.");
	}



	public static bool operator ==(Number left, Number right) {
		return left.Equals(right);
	}

	public static bool operator !=(Number left, Number right) {
		return !(left == right);
	}

	public static bool operator >(Number left, Number right) {
		return left.CompareTo(right) > 0;
	}

	public static bool operator <(Number left, Number right) {
		return left.CompareTo(right) < 0;
	}

	public static bool operator >=(Number left, Number right) {
		return left.CompareTo(right) >= 0;
	}

	public static bool operator <=(Number left, Number right) {
		return left.CompareTo(right) <= 0;
	}

}