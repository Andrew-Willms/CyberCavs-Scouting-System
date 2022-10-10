using System;
using System.Collections.Generic;

namespace UtilitiesLibrary;



public class Optional {

	private Optional() { }

	public static readonly Optional NoValue = new();
}



public class Optional<T> {

	private readonly T _Value;
	public T Value {

		get {

			if (!HasValue) {
				throw new EvaluatingValuelessOptionalException();
			}

			return _Value;
		}
	}

	public bool HasValue { get; }



	private Optional() {

		HasValue = false;
		_Value = default!;
	}

	public Optional(T value) {

		HasValue = true;
		_Value = value;
	}

	private static readonly Optional<T> NoValue = new();

	public static implicit operator Optional<T>(Optional _) {
		return NoValue;
	}



	private bool Equals(Optional<T> other) {
		return EqualityComparer<T>.Default.Equals(_Value, other._Value) && HasValue == other.HasValue;
	}

	public override bool Equals(object? obj) {

		if (ReferenceEquals(null, obj)) {
			return false;
		}

		if (ReferenceEquals(this, obj)) {
			return true;
		}

		return obj.GetType() == GetType() && Equals((Optional<T>)obj);
	}

	public override int GetHashCode() {
		return HashCode.Combine(_Value, HasValue);
	}

	public static bool operator ==(Optional<T> left, T? right) {

		return left.HasValue && left.Value!.Equals(right);
	}

	public static bool operator !=(Optional<T> left, T? right) {

		return !(left == right);
	}

	public static bool operator ==(T? left, Optional<T> right) {

		return (right == left);
	}

	public static bool operator !=(T? left, Optional<T> right) {

		return !(left == right);
	}

	public static bool operator ==(Optional<T> left, Optional<T> right) {

		if (!left.HasValue) {
			return !right.HasValue;
		}

		return right.HasValue && left.Value!.Equals(right.Value);
	}

	public static bool operator !=(Optional<T> left, Optional<T> right) {
		return !(left == right);
	}

}



public class EvaluatingValuelessOptionalException : InvalidOperationException {

	public EvaluatingValuelessOptionalException() { }

	public EvaluatingValuelessOptionalException(string message) : base(message) { }

	public EvaluatingValuelessOptionalException(string message, Exception inner) : base(message, inner) { }
}