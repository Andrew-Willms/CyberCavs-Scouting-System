using System.Windows.Media;

namespace WPFUtilities.Extensions;



public static class ColorExtensions {

	public static int Difference(this Color color, Color otherColor) {

		return System.Math.Abs(color.R - otherColor.R) + 
		       System.Math.Abs(color.G - otherColor.G) + 
		       System.Math.Abs(color.B - otherColor.B);
	}

}