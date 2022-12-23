using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators; 



public static class VersionValidationData {

	public static class ComponentNumber {
			
		public const string EntryInstruction = "Please enter a positive integer.";
		public const string EnteredText = "A positive integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"A version component number must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a version" +
			                                                   $" component number. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"A version component number cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"A version component number must be a whole number.");

		public static readonly WholeConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			CannotBeNegativeError = CannotBeNegativeError,
			MustBeIntegerError = MustBeIntegerError
		};

	}

}