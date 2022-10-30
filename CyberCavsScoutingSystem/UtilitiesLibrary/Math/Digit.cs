using System;
using System.ComponentModel;
using System.Numerics;
using UtilitiesLibrary.SmartEnum;

namespace UtilitiesLibrary.Math; 



public class Digit : OrderedSmartEnum<Digit> {

	public static readonly Digit Zero = new (nameof(Zero), 0);
	public static readonly Digit One = new (nameof(One), 1);
	public static readonly Digit Two = new (nameof(Two), 2);
	public static readonly Digit Three = new (nameof(Three), 3);
	public static readonly Digit Four = new (nameof(Four), 4);
	public static readonly Digit Five = new (nameof(Five), 5);
	public static readonly Digit Six = new (nameof(Six), 6);
	public static readonly Digit Seven = new (nameof(Seven), 7);
	public static readonly Digit Eight = new (nameof(Eight), 8);
	public static readonly Digit Nine = new (nameof(Nine), 9);

	private Digit(string name, int value) : base(name, value) { }


	
	public static T ToINumber<T>(Digit digit) where T : INumber<T> {

		return digit.Value switch {
			0 => T.Zero,
			1 => T.One,
			2 => Constants.Numbers<T>.Two,
			3 => Constants.Numbers<T>.Three,
			4 => Constants.Numbers<T>.Four,
			5 => Constants.Numbers<T>.Five,
			6 => Constants.Numbers<T>.Six,
			7 => Constants.Numbers<T>.Seven,
			8 => Constants.Numbers<T>.Eight,
			9 => Constants.Numbers<T>.Nine,
			_ => throw new InvalidEnumArgumentException()
		};
	}

	public static Digit FromINumber<T>(T number) where T : INumber<T> {

		if (number < T.Zero) {
			number *= Constants.Numbers<T>.MinusOne;
		}

		if (number == T.Zero) return Zero;
		if (number == T.One) return One;
		if (number == Constants.Numbers<T>.Two) return Two;
		if (number == Constants.Numbers<T>.Three) return Three;
		if (number == Constants.Numbers<T>.Four) return Four;
		if (number == Constants.Numbers<T>.Five) return Five;
		if (number == Constants.Numbers<T>.Six) return Six;
		if (number == Constants.Numbers<T>.Seven) return Seven;
		if (number == Constants.Numbers<T>.Eight) return Eight;
		if (number == Constants.Numbers<T>.Nine) return Nine;

		throw new InvalidOperationException(
			"You are trying to convert a INumber that is not a single digit to a Digit enum.");
	}



	public static Digit FromChar(char character) {

		return character switch {
			'0' => Zero,
			'1' => One,
			'2' => Two,
			'3' => Three,
			'4' => Four,
			'5' => Five,
			'6' => Six,
			'7' => Seven,
			'8' => Eight,
			'9' => Nine,
			_ => throw new InvalidOperationException("You can't convert a non-digit character to a Digit enum.")
		};

	}



	public static Digit GetOnesColumn<T>(T number) where T : INumber<T> {

		T onesColumnAndDecimals = number % Constants.Numbers<T>.Ten;

		T onesColumn = onesColumnAndDecimals - number % T.One;

		return FromINumber(onesColumn);
	}

}