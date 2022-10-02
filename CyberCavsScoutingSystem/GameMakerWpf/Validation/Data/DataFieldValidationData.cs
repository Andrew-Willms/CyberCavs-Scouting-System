using CCSSDomain;
using GameMakerWpf.Validation.Conversion;
using UtilitiesLibrary;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data; 



public static class DataFieldValidationData {

	public static class Name {

		public static class Length {

			public const int LowerErrorThreshold = 0;
			public const int LowerWarningThreshold = 2;
			public const int LowerAdvisoryThreshold = 4;

			public const int UpperErrorThreshold = 1000;
			public const int UpperWarningThreshold = 30;
			public const int UpperAdvisoryThreshold = 20;



			public static readonly Error TooShortError =
				new("Empty Name", ErrorSeverity.Error, "An data field cannot have a blank name.");

			public static readonly Error TooShortWarning =
				new("Short Name", ErrorSeverity.Warning, "The data field name is alarmingly short.");

			public static readonly Error TooShortAdvisory =
				new("Short Name", ErrorSeverity.Advisory, "The data field name is rather short.");



			public static readonly Error TooLongError
				= new("Long Name", ErrorSeverity.Error, $"An data field name cannot be more than {UpperErrorThreshold} characters.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The data field name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Advisory, "The data field name is rather long.");

		}

		public static Error GetDuplicateNameError(string name) {

			return new("Duplicate Name", ErrorSeverity.Error, $"Multiple data fields have the name {name}.");
		}

	}

}