using System;
using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Validation.Data; 



public static class CommonValidationData {

	public static Error GetInvalidCharactersError(char[] invalidCharacters) {

		string invalidMessage = invalidCharacters.Length switch {
			0 => throw new ArgumentException($"It is expected that {nameof(invalidCharacters)} has at least one item."),
			1 => $"The character \"{invalidCharacters}\" is not valid.",
			_ => $"The characters \"{invalidCharacters}\" are not valid. "
		};

		return new("Invalid Characters", ErrorSeverity.Error, invalidMessage);
	}

}
