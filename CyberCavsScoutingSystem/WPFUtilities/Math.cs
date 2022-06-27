using System;

namespace WPFUtilities; 



public static class Math {

	public static T Max<T>(T left, T right) where T : IComparable {

		return left.CompareTo(right) > 0 ? left : right;
	}

}