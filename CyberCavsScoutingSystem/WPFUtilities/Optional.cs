using System;

namespace WPFUtilities;



public class Optional {

	private Optional() { }

	public static readonly Optional NoValue = new();
}



public class Optional<T> {

	private readonly T _Value;
	public T Value {

		get {
			if (!HasValue) {
				// TODO: make this a custom exception
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

	private static readonly Optional<T> NoValue = new();

	public static implicit operator Optional<T>(Optional _) {
		return NoValue;
	}

	public static implicit operator Optional<T>(T value) {
		return new(value);
	}

}