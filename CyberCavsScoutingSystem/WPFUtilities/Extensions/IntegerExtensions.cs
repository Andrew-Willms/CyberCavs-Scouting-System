using System;
using System.Windows.Media;

namespace WPFUtilities.Extensions; 



public static class IntegerExtensions {
	
	public static string ToWrittenConvention(this int integer) {

		return integer switch {
			< 0 => "negative " + (-integer).ToWrittenConvention(),
			1 => "one",
			2 => "two",
			3 => "three",
			4 => "four",
			5 => "five",
			6 => "six",
			7 => "seven",
			8 => "eight",
			9 => "nine",
			>= 10 => integer.ToString(),
			_ => throw new Exception("I am not sure how you managed to trigger this.")
		};
	}

}