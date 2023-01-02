using WindowsColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;

namespace WPFUtilities;



public static class ColorExtensions {

	/// <summary>
	/// Calculates the 3-dimensional distance between two colors in the RBG color volume.
	/// </summary>
	/// <param name="color"></param>
	/// <param name="otherColor"></param>
	/// <returns>The 3-dimensional distance between two colors in the RBG color volume.</returns>
	public static double Difference(this WindowsColor color, WindowsColor otherColor) {

		double deltaR = System.Math.Abs(color.R - otherColor.R);
		double deltaG = System.Math.Abs(color.G - otherColor.G);
		double deltaB = System.Math.Abs(color.B - otherColor.B);

		return System.Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB);
	}

	public static WindowsColor ToWindowsColor(this DrawingColor drawingColor) {

		return WindowsColor.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
	}

	public static DrawingColor ToDrawingColor(this WindowsColor windowsColor) {

		return DrawingColor.FromArgb(windowsColor.A, windowsColor.R, windowsColor.G, windowsColor.B);
	}

}