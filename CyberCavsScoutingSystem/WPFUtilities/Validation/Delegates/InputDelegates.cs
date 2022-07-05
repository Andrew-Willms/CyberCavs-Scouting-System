using System;

namespace WPFUtilities.Validation.Delegates;



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



/*
When value types are used in StringInputValidator<TheValueType, TSeverityEnum> the method cannot return a nullable
version of that value type because C# handles nullable reference types and nullable value types differently
*/