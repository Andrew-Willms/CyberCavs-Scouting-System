using System;
using System.Numerics;
using UtilitiesLibrary.SmartEnum;
using UtilitiesLibrary.Validation;

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



	public byte ToByte() {

		if (this == Zero) return 0;
		if (this == One) return 1;
		if (this == Two) return 2;
		if (this == Three) return 3;
		if (this == Four) return 4;
		if (this == Five) return 5;
		if (this == Six) return 6;
		if (this == Seven) return 7;
		if (this == Eight) return 8;
		if (this == Nine) return 9;

		throw new ShouldNotReachException();
	}

	public static Digit FromByte(byte number) {

		return number switch {
			0 => Zero,
			1 => One,
			2 => Two,
			3 => Three,
			4 => Four,
			5 => Five,
			6 => Six,
			7 => Seven,
			8 => Eight,
			9 => Nine,
			_ => throw new ShouldMatchOtherCaseException()
		};
	}

	public static Digit FromINumber<T>(T number) where T : INumber<T> {

		if (number == T.Zero) return Zero;
		if (number == T.One) return One;
		if (number == Constants.NumberInterface<T>.Two) return Two;
		if (number == Constants.NumberInterface<T>.Three) return Three;
		if (number == Constants.NumberInterface<T>.Four) return Four;
		if (number == Constants.NumberInterface<T>.Five) return Five;
		if (number == Constants.NumberInterface<T>.Six) return Six;
		if (number == Constants.NumberInterface<T>.Seven) return Seven;
		if (number == Constants.NumberInterface<T>.Eight) return Eight;
		if (number == Constants.NumberInterface<T>.Nine) return Nine;

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

		T ten = T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One + T.One;

		T lowestValue = number % ten;
		return FromINumber(lowestValue);
	}

}