using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace UtilitiesLibrary;



public static class ColorsHelper {

	private static ReadOnlyList<(string, Color)>? _DefaultColors;
	public static ReadOnlyList<(string, Color)> DefaultColors => _DefaultColors ??= GetDefaultColors();

	private static ReadOnlyList<(string, Color)> GetDefaultColors() {

		return typeof(Colors)
			.GetProperties().
			Where(x => x.PropertyType == typeof(Color)).
			Select(x => (StringExtensions.SpaceCamelCaseName(x.Name), (Color)x.GetValue(null)!))
			.ToReadOnly();
	}

	public static List<(string, Color)> DefaultColorsRandomized() {

		Random random = new();
		return new(DefaultColors.OrderBy(_ => random.Next()));
	}

	//public static Color[] AllColors = {
	//	Colors.Orchid,
	//	Colors.Orange,
	//	Colors.Olive,
	//	Colors.OldLace,
	//	Colors.Navy,
	//	Colors.NavajoWhite,
	//	Colors.Moccasin,
	//	Colors.MistyRose,
	//	Colors.MidnightBlue,
	//	Colors.MediumVioletRed,
	//	Colors.MediumTurquoise,
	//	Colors.MediumSpringGreen,
	//	Colors.MediumSlateBlue,
	//	Colors.LightSkyBlue,
	//	Colors.LightSlateGray,
	//	Colors.LightSteelBlue,
	//	Colors.LightYellow,
	//	Colors.Lime,
	//	Colors.Maroon,
	//	Colors.MediumAquamarine,
	//	Colors.MediumBlue,
	//	Colors.MediumOrchid,
	//	Colors.MediumPurple,
	//	Colors.MediumSeaGreen,
	//	Colors.Magenta,
	//	Colors.PaleTurquoise,
	//	Colors.PaleVioletRed,
	//	Colors.PapayaWhip,
	//	Colors.SlateGray,
	//	Colors.SpringGreen,
	//	Colors.SteelBlue,
	//	Colors.Tan,
	//	Colors.Teal,
	//	Colors.SlateBlue,
	//	Colors.Thistle,
	//	Colors.Turquoise,
	//	Colors.Violet,
	//	Colors.Wheat,
	//	Colors.White,
	//	Colors.WhiteSmoke,
	//	Colors.Tomato,
	//	Colors.LightSeaGreen,
	//	Colors.SkyBlue,
	//	Colors.Sienna,
	//	Colors.PeachPuff,
	//	Colors.Peru,
	//	Colors.Pink,
	//	Colors.Plum,
	//	Colors.PowderBlue,
	//	Colors.Purple,
	//	Colors.Silver,
	//	Colors.Red,
	//	Colors.RoyalBlue,
	//	Colors.SaddleBrown,
	//	Colors.Salmon,
	//	Colors.SandyBrown,
	//	Colors.SeaGreen,
	//	Colors.SeaShell,
	//	Colors.RosyBrown,
	//	Colors.Yellow,
	//	Colors.LightSalmon,
	//	Colors.LightGreen,
	//	Colors.DarkRed,
	//	Colors.DarkOrchid,
	//	Colors.DarkOrange,
	//	Colors.DarkOliveGreen,
	//	Colors.DarkMagenta,
	//	Colors.DarkKhaki,
	//	Colors.DarkGreen,
	//	Colors.DarkGray,
	//	Colors.DarkGoldenrod,
	//	Colors.DarkCyan,
	//	Colors.DarkBlue,
	//	Colors.Cyan,
	//	Colors.Crimson,
	//	Colors.Cornsilk,
	//	Colors.CornflowerBlue,
	//	Colors.Coral,
	//	Colors.Chocolate,
	//	Colors.AntiqueWhite,
	//	Colors.Aqua,
	//	Colors.Aquamarine,
	//	Colors.Azure,
	//	Colors.Beige,
	//	Colors.Bisque,
	//	Colors.DarkSalmon,
	//	Colors.Black,
	//	Colors.Blue,
	//	Colors.BlueViolet,
	//	Colors.Brown,
	//	Colors.BurlyWood,
	//	Colors.CadetBlue,
	//	Colors.Chartreuse,
	//	Colors.BlanchedAlmond,
	//	Colors.DarkSeaGreen,
	//	Colors.DarkSlateBlue,
	//	Colors.DarkSlateGray,
	//	Colors.HotPink,
	//	Colors.IndianRed,
	//	Colors.Indigo,
	//	Colors.Ivory,
	//	Colors.Khaki,
	//	Colors.Lavender,
	//	Colors.Honeydew,
	//	Colors.LavenderBlush,
	//	Colors.LemonChiffon,
	//	Colors.LightBlue,
	//	Colors.LightCoral,
	//	Colors.LightCyan,
	//	Colors.LightGoldenrodYellow,
	//	Colors.LightGray,
	//	Colors.LawnGreen,
	//	Colors.LightPink,
	//	Colors.GreenYellow,
	//	Colors.Gray,
	//	Colors.DarkTurquoise,
	//	Colors.DarkViolet,
	//	Colors.DeepPink,
	//	Colors.DeepSkyBlue,
	//	Colors.DimGray,
	//	Colors.DodgerBlue,
	//	Colors.Green,
	//	Colors.Firebrick,
	//	Colors.ForestGreen,
	//	Colors.Fuchsia,
	//	Colors.Gainsboro,
	//	Colors.GhostWhite,
	//	Colors.Gold,
	//	Colors.Goldenrod,
	//	Colors.FloralWhite,
	//	Colors.YellowGreen,
	//};

}