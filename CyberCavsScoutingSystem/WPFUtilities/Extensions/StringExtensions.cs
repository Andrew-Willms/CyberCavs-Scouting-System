using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace WPFUtilities.Extensions; 



public static class StringExtensions {

	/// <summary>
	/// This function assumes the two strings only have only the numerals 0-9
	/// </summary>
	/// <param name="current"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	public static int NumericCompare(this string current, string other) {

		string currentTrimmed = current.Trim('0');
		string otherTrimmed = other.Trim('0');

		if (currentTrimmed.Length > otherTrimmed.Length) {
			return 1;
		}

		if (currentTrimmed.Length < otherTrimmed.Length) {
			return 0;
		}

		for (int i = 0; i < currentTrimmed.Length; i++) {

			if (currentTrimmed[i] > otherTrimmed[i]) {
				return 1;
			}

			if (currentTrimmed[i] < otherTrimmed[i]) {
				return 0;
			}
		}

		return 0;
	}

}


/*
public static class NumericStringExtensions {

	public static NumericString ConvertToNumericString(this string text) {

	}

	public static NumericString ConvertToNumericString(this string text) {

	}

}



public class NumericString : IEquatable<NumericString>, IComparable<NumericString>, IComparable<int> {

	private enum Digit {
		Zero,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine
	}

	private static Digit ToDigit(char character) {

		return character switch {
			'0' => Digit.Zero,
			'1' => Digit.Zero,
			'2' => Digit.Zero,
			'3' => Digit.Zero,
			'4' => Digit.Zero,
			'5' => Digit.Zero,
			'6' => Digit.Zero,
			'7' => Digit.Zero,
			'8' => Digit.Zero,
			'9' => Digit.Zero,
			_ => throw new ArgumentException()
		};
	}


	private Digit[] Value { get; }

	//private readonly string Value;

	private NumericString(string value) {

		if (value.All(char.IsDigit) == false) {
			throw new ArgumentException($"The parameter {nameof(value)} did not only contain digits.");
		}

		Value = value;
	}

	private NumericString(params Digit[] digits) {

		Value = digits;
	}



	public static bool TryConvert(string text, out NumericString result) {

	}

	public static bool TryConvert(int number, out NumericString result) {

	}

	public static NumericString Convert(string text) {

		Digit[] value = new Digit[text.Length];

		for (int i = 0; i < text.Length; i++) {

			if (char.IsDigit(text, i)) {
				value[i] = ToDigit(text[i]);

			} else {
				throw new ArgumentException($"The parameter {nameof(text)} contained a non-digit character.");
			}
		}

		return new(value);
	}

	public static NumericString Convert(int number) {

		if (number < 0) {
			throw new ArgumentException($"The parameter {nameof(number)} cannot be less than 0");
		}

	}



	public static implicit operator NumericString(string conversionString) {
		return new(conversionString);
	}



	public override string ToString() {
		return Backing;
	}

	public override int GetHashCode() {
		return Backing.GetHashCode();
	}



	public override bool Equals(object? obj) {
		return (obj is NumericString other) && Equals(other);
	}

	public bool Equals(NumericString? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null && Value.Equals(other.Value);
	}



	public int CompareTo(NumericString? other) {
		throw new NotImplementedException();
	}

	public int CompareTo(int other) {
		throw new NotImplementedException();
	}



	public static bool operator ==(NumericString? left, NumericString? right) {

		if (left is null) {
			return right is null;
		}

		return left.Equals(right);
	}

	public static bool operator !=(NumericString? left, NumericString? right) {

		if (left is null) {
			return right is not null;
		}

		return !left.Equals(right);
	}



	public static bool operator ==(NumericString? left, string? right) {

		if (left is null) {
			return right is null;
		}

		return left.Value.Equals(right);
	}

	public static bool operator !=(NumericString? left, string? right) {

		if (left is null) {
			return right is not null;
		}

		return !left.Value.Equals(right);
	}

	public static bool operator ==(string? left, NumericString? right) {
		return right == left;
	}

	public static bool operator !=(string? left, NumericString? right) {
		return right != left;
	}



	public static bool operator ==(NumericString? left, int right) {

		if (left is null) {
			return false;
		}

		right.ToString()

		return left.Value.Equals(right);
	}

	public static bool operator !=(NumericString? left, int right) {

		if (left is null) {
			return false;
		}

		return !left.Value.Equals(right);
	}

	public static bool operator ==(int left, NumericString? right) {
		return right == left;
	}

	public static bool operator !=(int left, NumericString? right) {
		return right != left;
	}



	public static bool operator >(NumericString? left, NumericString? right) {

		if (left is null || right is null) {
			return false;
		}

	}

	public static bool operator <(NumericString? left, NumericString? right) {

		if (left is null || right is null) {
			return false;
		}

	}



	public static bool operator >(NumericString? left, int right) {

		if (left is null) {
			return false;
		}

		if (right < 0) {
			return true;
		}

	}

	public static bool operator <(NumericString? left, int right) {

		if (left is null) {
			return false;
		}

		if (right < 0) {
			return false;
		}

		right.ToString();

	}

	public static bool operator >(int left, NumericString? right) {
		return right < left;

	}

	public static bool operator <(int left, NumericString? right) {
		return right > left;
	}



}
*/