using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace WPFUtilities; 



// I got the idea for this from the Nick Chapsas video https://www.youtube.com/watch?v=CEZ6cF8eoeg
// and Steve Smith's nuget package https://github.com/ardalis/SmartEnum.

public abstract class SmartEnum<T> {

	public string Name { get; }
	public int Value { get; }

	protected SmartEnum(string name, int value) {

		if (string.IsNullOrEmpty(name)) {
			throw new ArgumentException($"The parameter \"{nameof(name)}\" is null or empty.", nameof(name));
		}

		Name = name;
		Value = value;
	}

	//public abstract ReadOnlyCollection<SmartEnum<T>> Options { get; }

	public static SmartEnum<T>[] GetAllOptions() {

		Type baseType = typeof(T);

		Assembly assembly = baseType.Assembly;

		Type[] assemblyTypes = assembly.GetTypes();

		IEnumerable<Type> relevantTypes = assemblyTypes.Where(x => baseType.IsAssignableFrom(x));

		IEnumerable<FieldInfo> fieldInfos = relevantTypes.SelectMany(x => x.GetFields());

		IEnumerable<FieldInfo> relevantFields = fieldInfos.Where(x => x.FieldType == typeof(T));

		IEnumerable<SmartEnum<T>?> smartEnums = relevantFields.Select(x => x.GetValue(null) as SmartEnum<T>);

		IEnumerable<SmartEnum<T>> mullSafeSmartEnums = smartEnums.Where(x => x is not null)!;

		IEnumerable<SmartEnum<T>> alphabeticalSmartEnums = mullSafeSmartEnums.OrderBy(x => x.Name);

		return alphabeticalSmartEnums.ToArray();

		//return assembly.GetTypes()
		//	.Where(x => baseType.IsAssignableFrom(x))
		//	.SelectMany(x => x.GetFields())
		//	.Where(x => x.FieldType == typeof(T))
		//	.Select(x => (SmartEnum<T>)x.GetValue(null))
		//	.OrderBy(x => x.Name)
		//	.ToArray();
	}

	public override string ToString() {
		return $"{nameof(SmartEnum<T>)}.{Name}";
	}

	public override int GetHashCode() {
		return Value.GetHashCode();
	}

	public override bool Equals(object? obj) {
		return (obj is SmartEnum<T> other) && Equals(other);
	}

	public bool Equals(SmartEnum<T>? other) {

		if (ReferenceEquals(this, other)) {
			return true;
		}

		if (other is null) {
			return false;
		}

		return Value.Equals(other.Value);
	}

	public static bool operator ==(SmartEnum<T>? left,SmartEnum<T>? right) {

		if (left is null) {
			return right is null;
		}

		return !(left != right);
	}

	public static bool operator !=(SmartEnum<T>? left, SmartEnum<T>? right) {
		return !(left == right);
	}

}

public abstract class OrderedSmartEnum<T> : SmartEnum<T> {

	protected OrderedSmartEnum(string name, int value) : base(name, value) { }

	//public static OrderedSmartEnum<T>[] GetAllOptions() {

	//	return (SmartEnum<T>.GetAllOptions().OrderBy(x => x.Value).Select(x => x as OrderedSmartEnum<T>).ToArray());

	//}

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