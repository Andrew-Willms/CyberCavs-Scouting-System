using System;

namespace WPFUtilities; 



public struct Optional<T> {

	private T? _Value = default;
	public T Value {

		get {
			if (!HasValue) {
				throw new InvalidOperationException("You cannot get the value of a null nullable.");
			}

			return _Value;
		}

		set => _Value = value;
	}

	public bool HasValue { get; }

	public Optional() {

		HasValue = false;
	}

	public Optional(T value) {

		HasValue = true;
		Value = value;
	}

}
