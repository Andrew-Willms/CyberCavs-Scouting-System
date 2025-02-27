﻿using System;
using System.Collections.Generic;

namespace UtilitiesLibrary.Optional;



public class Optional {

	private Optional() { }

	public static readonly Optional NoValue = new();

}



public class Optional<T> : IEquatable<Optional<T>> {

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

	public static readonly Optional<T> NoValue = new();

	public static implicit operator Optional<T>(Optional noValue) {

		if (noValue != Optional.NoValue) {
			throw new ArgumentException($"This casting operator should only be used with {nameof(Optional.NoValue)}", nameof(noValue));
		}

		return NoValue;
	}

	public static implicit operator Optional<T>(T value) {

		return new(value);
	}





	public bool Equals(Optional<T>? other) {

		if (other is null) {
			return false;
		}

		if (ReferenceEquals(this, other)) {
			return true;
		}

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

		return right == left;
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



	public override string ToString() {

		return (HasValue
			? $"Optional<{GetType().GetGenericArguments()[0].Name}> {{ {Value} }}"
			: $"Optional<{GetType().GetGenericArguments()[0].Name}> {{ }}");
	}

}



public class EvaluatingValuelessOptionalException : InvalidOperationException {

	public EvaluatingValuelessOptionalException() { }

	public EvaluatingValuelessOptionalException(string message) : base(message) { }

	public EvaluatingValuelessOptionalException(string message, Exception inner) : base(message, inner) { }
}