using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFUtilities; 



public static class Math {

	public static T Max<T>(T left, T right) where T : IComparable {

		return left.CompareTo(right) > 0 ? left : right;
	}

	public static T Max<T>(params T[] items) where T : IComparable {

		if (items.Length == 0) {
			throw new ArgumentException("");
		}

		return items.Max() ?? throw new NullReferenceException("Somehow the max is null.");
	}

}