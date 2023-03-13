using System.Text;

namespace UtilitiesLibrary.MiscExtensions;



public static class TextExtensions {

	public static string ToWrittenConvention(this int integer) {

		return integer switch {
			< 0 => "negative " + (-integer).ToWrittenConvention(),
			0 => "zero",
			1 => "one",
			2 => "two",
			3 => "three",
			4 => "four",
			5 => "five",
			6 => "six",
			7 => "seven",
			8 => "eight",
			9 => "nine",
			>= 10 => integer.ToString()
		};
	}

	public static string SpaceCamelCaseName(string text) {

		if (string.IsNullOrWhiteSpace(text)) {
			return "";
		}

		StringBuilder spacedText = new();

		spacedText.Append(text[0]);

		for (int i = 1; i < text.Length; i++) {

			if (char.IsUpper(text[i])) {
				spacedText.Append(' ');
			}

			spacedText.Append(text[i]);
		}

		return spacedText.ToString();
	}

    public static string ToCsvFriendly(this string text) {

        text = text.Replace("\"", "\"\"");
        text = "\"" + text + "\"";
        return text;
    }

}