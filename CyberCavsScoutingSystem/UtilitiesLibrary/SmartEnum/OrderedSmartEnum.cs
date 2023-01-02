using System;

namespace UtilitiesLibrary.SmartEnum;



public abstract class OrderedSmartEnum<T> : SmartEnum<T>, IComparable<OrderedSmartEnum<T>> where T : OrderedSmartEnum<T> {

	protected OrderedSmartEnum(string name, int value) : base(name, value) { }

	public int CompareTo(OrderedSmartEnum<T>? other) {

		ArgumentNullException.ThrowIfNull(other, nameof(other));

		return Value.CompareTo(other.Value);
	}

	public static bool operator <(OrderedSmartEnum<T>? left, OrderedSmartEnum<T>? right) {

		ArgumentNullException.ThrowIfNull(left, nameof(left));
		ArgumentNullException.ThrowIfNull(right, nameof(right));

		return left.CompareTo(right) < 0;
	}

	public static bool operator >(OrderedSmartEnum<T>? left, OrderedSmartEnum<T>? right) {

		ArgumentNullException.ThrowIfNull(left, nameof(left));
		ArgumentNullException.ThrowIfNull(right, nameof(right));

		return left.CompareTo(right) > 0;
	}

	public static bool operator <=(OrderedSmartEnum<T>? left, OrderedSmartEnum<T>? right) {

		ArgumentNullException.ThrowIfNull(left, nameof(left));
		ArgumentNullException.ThrowIfNull(right, nameof(right));

		return left.CompareTo(right) <= 0;
	}

	public static bool operator >=(OrderedSmartEnum<T> left, OrderedSmartEnum<T>? right) {

		ArgumentNullException.ThrowIfNull(left, nameof(left));
		ArgumentNullException.ThrowIfNull(right, nameof(right));

		return left.CompareTo(right) >= 0;
	}

}