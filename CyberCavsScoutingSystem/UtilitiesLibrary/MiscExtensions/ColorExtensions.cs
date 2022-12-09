using System.Windows.Media;

namespace UtilitiesLibrary.MiscExtensions;



public static class ColorExtensions {

	/// <summary>
	/// Calculates the 3-dimensional distance between two colors in the RBG color volume.
	/// </summary>
	/// <param name="color"></param>
	/// <param name="otherColor"></param>
	/// <returns>The 3-dimensional distance between two colors in the RBG color volume.</returns>
	public static double Difference(this Color color, Color otherColor) {

		double deltaR = System.Math.Abs(color.R - otherColor.R);
		double deltaG = System.Math.Abs(color.G - otherColor.G);
		double deltaB = System.Math.Abs(color.B - otherColor.B);

		return System.Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB);
	}

}