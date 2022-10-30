﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using UtilitiesLibrary.MiscExtensions;

namespace UtilitiesLibrary.Math.Numbers;



public class Integer : Whole, IEquatable<Integer>, IComparable<Integer> {

	private bool IsNegative { get; }



	private Integer(bool isNegative, ReadOnlyCollection<Digit> digits) : base(digits) {
		IsNegative = isNegative;
	}

	private Integer(bool isNegative, Whole whole) : base(whole) {
		IsNegative = isNegative;
	}



	private new static Integer FromINumber<T>(T value) where T : INumber<T> {

		bool isNegative = value < T.Zero;

		List<Digit> digits = new();
		while (value >= T.One || value <= Constants.Numbers<T>.MinusOne) {

			digits.Add(Digit.GetOnesColumn(value));

			value /= Constants.Numbers<T>.Ten;
		}

		return new(isNegative, digits.ToReadOnly());
	}

	public static implicit operator Integer(byte value) => FromINumber(value);
	public static implicit operator Integer(ushort value) => FromINumber(value);
	public static implicit operator Integer(uint value) => FromINumber(value);
	public static implicit operator Integer(ulong value) => FromINumber(value);
	public static implicit operator Integer(short value) => FromINumber(value);
	public static implicit operator Integer(int value) => FromINumber(value);
	public static implicit operator Integer(long value) => FromINumber(value);



	private Result<T, IntegerToPrimitiveError> ToNumberPrimitive<T>(
		Integer typeMinValue, Integer typeMaxValue, Func<Digit, T> digitToT, Func<int, T> tenToThe) where T : INumber<T> {

		if (this < typeMinValue) {
			return new IntegerToPrimitiveError { ErrorType = IntegerToPrimitiveError.Types.ValueBelowMin };
		}

		if (this > typeMaxValue) {
			return new IntegerToPrimitiveError { ErrorType = IntegerToPrimitiveError.Types.ValueAboveMax };
		}

		T value = T.Zero;
		for (int position = 0; position < Digits.Count; position++) {
			value += digitToT(Digits[position]) * tenToThe(position);
		}

		if (IsNegative) {
			value *= Constants.Numbers<T>.MinusOne;
		}

		return value;
	}

	public static Result<byte, IntegerToPrimitiveError> ToByte(Integer integer) {

		return integer.ToNumberPrimitive(byte.MinValue, byte.MaxValue, Digit.ToINumber<byte>, i => (byte)System.Math.Pow(10, i));
	}

	public static Result<ushort, IntegerToPrimitiveError> ToUshort(Integer integer) {

		return integer.ToNumberPrimitive(ushort.MinValue, ushort.MaxValue, Digit.ToINumber<ushort>, i => (ushort)System.Math.Pow(10, i));
	}

	public static Result<uint, IntegerToPrimitiveError> ToUint(Integer integer) {

		return integer.ToNumberPrimitive(uint.MinValue, uint.MaxValue, Digit.ToINumber<uint>, i => (uint)System.Math.Pow(10, i));
	}

	public static Result<ulong, IntegerToPrimitiveError> ToUlong(Integer integer) {

		return integer.ToNumberPrimitive(ulong.MinValue, ulong.MaxValue, Digit.ToINumber<ulong>, i => (ulong)System.Math.Pow(10, i));
	}

	public static Result<short, IntegerToPrimitiveError> ToShort(Integer integer) {

		return integer.ToNumberPrimitive(short.MinValue, short.MaxValue, Digit.ToINumber<short>, i => (short)System.Math.Pow(10, i));
	}

	public static Result<int, IntegerToPrimitiveError> ToInt(Integer integer) {

		return integer.ToNumberPrimitive(int.MinValue, int.MaxValue, Digit.ToINumber<int>, i => (int)System.Math.Pow(10, i));
	}

	public static Result<long, IntegerToPrimitiveError> ToLong(Integer integer) {

		return integer.ToNumberPrimitive(long.MinValue, long.MaxValue, Digit.ToINumber<long>, i => (long)System.Math.Pow(10, i));
	}



	public new static Integer? Parse(string? text) {

		if (text is null || text.Length == 0) {
			return null;
		}

		if (text.All(x => x == '0')) {
			return new Integer(false, new List<Digit> { Digit.Zero }.AsReadOnly());
		}

		if (text.Multiple('-')) {
			return null;
		}

		bool isNegative = false;

		if (text.Contains('-')) {
			if (!text.StartsWith('-')) {
				return null;
			}

			text = text[1..];
			isNegative = true;
		}

		text = text.TrimStart('0');

		List<Digit> digits = new();

		foreach (char character in text.Reverse()) {
			if (!char.IsDigit(character)) {
				return null;
			}

			digits.Add(Digit.FromChar(character));
		}

		return new(isNegative, digits.AsReadOnly());
	}



	public override int GetHashCode() {
		return HashCode.Combine(IsNegative, Digits);
	}

	public override bool Equals(object? obj) {
		return obj is Integer other && Equals(other);
	}

	public bool Equals(Integer? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null &&
		       IsNegative == other.IsNegative &&
		       Digits.SequenceEqual(other.Digits);
	}

	public int CompareTo(Integer? other) {

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

		if (Digits.Count > other.Digits.Count) {
			return 1;
		}

		if (Digits.Count < other.Digits.Count) {
			return -1;
		}

		for (int position = Digits.Count - 1; position >= 0; position--) {
			if (Digits[position] > other.Digits[position]) {
				return 1;
			}

			if (Digits[position] < other.Digits[position]) {
				return -1;
			}
		}

		throw new("Since Equality is short-circuit it should not be possible that the two values are the same and" +
		          " execution should not reach this point.");
	}



	public static bool operator ==(Integer left, Integer right) {
		return left.Equals(right);
	}

	public static bool operator !=(Integer left, Integer right) {
		return !(left == right);
	}

	public static bool operator >(Integer left, Integer right) {
		return left.CompareTo(right) > 0;
	}

	public static bool operator <(Integer left, Integer right) {
		return left.CompareTo(right) < 0;
	}

	public static bool operator >=(Integer left, Integer right) {
		return left.CompareTo(right) >= 0;
	}

	public static bool operator <=(Integer left, Integer right) {
		return left.CompareTo(right) <= 0;
	}

}