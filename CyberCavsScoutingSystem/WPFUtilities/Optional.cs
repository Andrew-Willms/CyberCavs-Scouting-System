using System;

namespace WPFUtilities; 



public struct Optional<T> {

	private readonly T _Value;
	public T Value {

		get {
			if (!HasValue) {
				throw new InvalidOperationException("You cannot get the value of an Option with no value.");
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

	public static readonly Optional<T> NoValue = new();
}