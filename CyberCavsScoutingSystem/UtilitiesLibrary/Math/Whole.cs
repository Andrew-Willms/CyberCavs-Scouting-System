using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;

namespace UtilitiesLibrary.Math; 



public class Whole : IEquatable<Whole>, IComparable<Whole> {

	protected ReadOnlyCollection<Digit> Digits { get; }
	protected int LargestDecimalPosition => Digits.Count - 1;

	protected Whole(ReadOnlyCollection<Digit> digits) {
		Digits = digits;
	}

	protected Whole(Whole whole) {
		Digits = whole.Digits;
	}

	protected Digit GetDigitInPosition(int position) {
		return Digits[position];
	}



	private static Whole ToNumeric<T>(T value) where T : INumber<T> {

		List<Digit> digits = new();

		while (value > T.Zero) {

			digits.Add(Digit.GetOnesColumn(value));

			value /= Constants.NumberInterface<T>.Ten;
		}

		return new(digits.AsReadOnly());
	}

	public static implicit operator Whole(byte value) => ToNumeric(value);
	public static implicit operator Whole(ushort value) => ToNumeric(value);
	public static implicit operator Whole(uint value) => ToNumeric(value);
	public static implicit operator Whole(ulong value) => ToNumeric(value);
	public static implicit operator Whole(short value) => ToNumeric(value);
	public static implicit operator Whole(int value) => ToNumeric(value);
	public static implicit operator Whole(long value) => ToNumeric(value);



	protected Result<T> ToNumberPrimitive<T>(
		Func<Whole, bool> greaterThanMaxValue,
		Func<Whole, bool> lessThanMinValue,
		Func<Digit, T> digitToT,
		Func<int, T> tenToThe) where T : INumber<T> {

		if (greaterThanMaxValue(this)) {
			return new Failure<T>(new ValueTooLargeError());
		}

		if (lessThanMinValue(this)) {
			return new Failure<T>(new ValueTooSmallError());
		}

		T value = T.Zero;
		for (int position = 0; position <= LargestDecimalPosition; position++) {
			value += digitToT(GetDigitInPosition(position)) * tenToThe(position);
		}

		return new Success<T>(value);
	}

	public static Result<byte> ToByte(Whole whole) {

		return whole.ToNumberPrimitive(
			number => number > byte.MaxValue,
			number => number < byte.MinValue,
			digit => digit.ToByte(), i => (byte)System.Math.Pow(10, i)
		);
	}

	public static Result<ushort> ToUshort(Whole whole) {
		
		return whole.ToNumberPrimitive(
			number => number > ushort.MaxValue,
			number => number < ushort.MinValue,
			digit => digit.ToByte(), i => (ushort)System.Math.Pow(10, i)
		);

	}

	public static Result<uint> ToUint(Whole whole) {
		
		return whole.ToNumberPrimitive(
			number => number > uint.MaxValue,
			number => number < uint.MinValue,
			digit => digit.ToByte(), i => (uint)System.Math.Pow(10, i)
		);
	}

	public static Result<ulong> ToUlong(Whole whole) {

		return whole.ToNumberPrimitive(
			number => number > ulong.MaxValue,
			number => number < ulong.MinValue,
			digit => digit.ToByte(), i => (ulong)System.Math.Pow(10, i)
		);
	}

	public static Result<short> ToShort(Whole whole) {

		return whole.ToNumberPrimitive(
			number => number > short.MaxValue,
			number => number < short.MinValue,
			digit => digit.ToByte(), i => (short)System.Math.Pow(10, i)
		);
	}

	public static Result<int> ToInt(Whole whole) {

		return whole.ToNumberPrimitive(
			number => number > int.MaxValue,
			number => number < int.MinValue,
			digit => digit.ToByte(), i => (int)System.Math.Pow(10, i)
		);
	}

	public static Result<long> ToLong(Whole whole) {

		return whole.ToNumberPrimitive(
			number => number > long.MaxValue,
			number => number < long.MinValue,
			digit => digit.ToByte(), i => (long)System.Math.Pow(10, i)
		);
	}



	public static Whole? Parse(string? text) {

		if (text is null || text.Length == 0) {
			return null;
		}

		if (text.All(x => x == '0')) {
			return new Whole(new List<Digit> { Digit.Zero }.AsReadOnly());
		}

		text = text.TrimStart('0');

		List<Digit> digits = new();

		foreach (char character in text.Reverse()) {

			if (!char.IsDigit(character)) {
				return null;
			}

			digits.Add(Digit.FromChar(character));
		}

		return new Whole(digits.AsReadOnly());
	}



	public override int GetHashCode() {
		return Digits.GetHashCode();
	}

	public override bool Equals(object? obj) {

		return obj is Whole other && Equals(other);
	}

	public bool Equals(Whole? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null && Digits.SequenceEqual(other.Digits);
	}

	public int CompareTo(Whole? other) {

		if (other is null) {
			throw new ArgumentNullException(nameof(other));
		}

		if (Equals(other)) {
			return 0;
		}

		if (LargestDecimalPosition > other.LargestDecimalPosition) {
			return 1;
		}

		if (LargestDecimalPosition < other.LargestDecimalPosition) {
			return -1;
		}

		for (int position = LargestDecimalPosition; position >= 0; position--) {

			if (GetDigitInPosition(position) > other.GetDigitInPosition(position)) {
				return 1;
			}

			if (GetDigitInPosition(position) < other.GetDigitInPosition(position)) {
				return -1;
			}
		}

		throw new("Since Equality is short-circuit it should not be possible that the two values are the same and" +
		          " execution should not reach this point.");
	}



	public static bool operator ==(Whole left, Whole right) {
		return left.Equals(right);
	}

	public static bool operator !=(Whole left, Whole right) {
		return !left.Equals(right);
	}

	public static bool operator >(Whole left, Whole right) {
		return left.CompareTo(right) > 0;
	}

	public static bool operator <(Whole left, Whole right) {
		return left.CompareTo(right) < 0;
	}

	public static bool operator >=(Whole left, Whole right) {
		return left.CompareTo(right) >= 0;
	}

	public static bool operator <=(Whole left, Whole right) {
		return left.CompareTo(right) <= 0;
	}

}