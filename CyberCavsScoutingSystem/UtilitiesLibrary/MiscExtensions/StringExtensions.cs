using System.Text;

namespace UtilitiesLibrary.MiscExtensions; 



public static class StringExtensions {

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

}