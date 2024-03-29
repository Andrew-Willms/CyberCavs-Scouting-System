﻿using System;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Collections;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data;



public static class CommonValidationData {

	public static Error GetInvalidCharactersError(char[] invalidCharacters) {

		string invalidMessage = invalidCharacters.Length switch {
			0 => throw new ArgumentException($"It is expected that {nameof(invalidCharacters)} has at least one item."),
			1 => $"The character \"{invalidCharacters.CharArrayToString()}\" is not valid.",
			_ => $"The characters \"{invalidCharacters.CharArrayToString()}\" are not valid. "
		};

		return new("Invalid Characters", ErrorSeverity.Error, invalidMessage);
	}

}
