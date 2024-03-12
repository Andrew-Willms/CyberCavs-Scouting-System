using System;
using System.Numerics;

namespace UtilitiesLibrary.Math;



public static class Numbers<T> where T : INumber<T> {

	public static readonly T Two = T.One + T.One;
	public static readonly T Three = Two + T.One;
	public static readonly T Four = Three + T.One;
	public static readonly T Five = Four + T.One;
	public static readonly T Six = Five + T.One;
	public static readonly T Seven = Six + T.One;
	public static readonly T Eight = Seven + T.One;
	public static readonly T Nine = Eight + T.One;
	public static readonly T Ten = Nine + T.One;

	public static T TenToThe(int exponent) {

		T value = T.One;

		Func<T, T> operation = exponent > 0
			? x => x * Ten
			: x => x / Ten;

		for (int i = 0; i < exponent; i++) {

			value = operation(value);
		}

		return value;
	}

	public static readonly T MinusOne = T.Zero - T.One;

	public static T Abs(T value) {

		return value > T.Zero
			? value
			: value * MinusOne;
	}
}