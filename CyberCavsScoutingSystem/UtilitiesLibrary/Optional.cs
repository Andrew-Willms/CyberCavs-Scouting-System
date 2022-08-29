﻿using System;
using Microsoft.VisualBasic.CompilerServices;

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

	public Optional() {

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

	public static implicit operator Optional<T>(T value) {
		return new(value);
	}

	public static Optional<T> FromValue(T value) {
		return new(value);
	}




	public static bool operator ==(Optional<T> left, Optional<T> right) {

		if (!left.HasValue) {
			return !right.HasValue;
		}

		return right.HasValue && left.Value.Equals(right.Value);
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