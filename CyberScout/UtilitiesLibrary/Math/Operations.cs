using System;
using System.Linq;

namespace UtilitiesLibrary.Math;



public static class Operations {

	public static T Max<T>(T left, T right) where T : IComparable<T> {

		return left.CompareTo(right) > 0 ? left : right;
	}

	public static T Max<T>(params T[] items) where T : IComparable<T> {

		if (items.Length == 0) {
			throw new ArgumentException("");
		}

		return items.Max() ?? throw new NullReferenceException("Somehow the max is null.");
	}

	public static byte Pow(byte x, byte y) {

		byte value = x;
		for (int i = 0; i < y - 1; i++) {
			x *= x;
		}

		return value;
	}

}