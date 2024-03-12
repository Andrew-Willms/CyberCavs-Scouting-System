using System;

namespace UtilitiesLibrary.SmartEnum;



// I got the idea for this from the Nick Chapsas video https://www.youtube.com/watch?v=CEZ6cF8eoeg
// and Steve Smith's nuget package https://github.com/ardalis/SmartEnum.
public abstract class SmartEnum<T> : IEquatable<SmartEnum<T>> where T : SmartEnum<T> {

	private string Name { get; }

	protected int Value { get; }

	protected SmartEnum(string name, int value) {

		if (string.IsNullOrEmpty(name)) {
			throw new ArgumentException($"The parameter \"{nameof(name)}\" is null or empty.", nameof(name));
		}

		Name = name;
		Value = value;
	}

	public override string ToString() {
		return $"{nameof(SmartEnum<T>)}.{Name}";
	}

	public override int GetHashCode() {
		return Value.GetHashCode();
	}

	public override bool Equals(object? obj) {
		return obj is SmartEnum<T> other && Equals(other);
	}

	public bool Equals(SmartEnum<T>? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		return other is not null && Value.Equals(other.Value);
	}

	public static bool operator ==(SmartEnum<T>? left, SmartEnum<T>? right) {

		if (left is null) {
			return right is null;
		}

		return left.Equals(right);
	}

	public static bool operator !=(SmartEnum<T>? left, SmartEnum<T>? right) {

		if (left is null) {
			return right is not null;
		}

		return !left.Equals(right);
	}

}