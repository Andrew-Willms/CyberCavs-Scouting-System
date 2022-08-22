using System.Collections.Generic;
using Xunit;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Tests.GameValidator;



public class Name {

	public class Converter {

		[Fact]
		public void NullNameShouldThrowException() {

			Assert.Throws<NullInputObjectInConverterException>(() => 
				Validation.Validators.GameTextValidator.NameConversionPair.Converter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= Validation.Validators.GameTextValidator.NameConversionPair.Converter(validName);

			Assert.True(optional.HasValue);
			Assert.Empty(errors);
			Assert.Equal(optional.Value, validName);
		}

	}

	public class Inverter {

		[Fact]
		public void NullNameShouldThrowException() {

			Assert.Throws<NullInputObjectInInverterException>(() => 
				Validation.Validators.GameTextValidator.NameConversionPair.Inverter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= Validation.Validators.GameTextValidator.NameConversionPair.Inverter(validName);

			Assert.True(optional.HasValue);
			Assert.Empty(errors);
			Assert.Equal(optional.Value, validName);
		}

	}

	public class LengthValidator {

		public static IEnumerable<object[]> LongNameShouldAdviseTestData() {

			yield return new object[] {
				"Some Really Long Name One Two Three"
			};
		}

		[Fact]
		public void EmptyNameShouldError() {

			Optional<Error> error = Validation.Validators.GameTextValidator.NameValidator_Length("");

			Assert.True(error.HasValue);
			Assert.Equal(error.Value, Validation.Data.GameValidationData.Name.Length.TooShortError);
		}


		[Theory]
		[MemberData(nameof(LongNameShouldAdviseTestData))]
		public void LongNameShouldAdvise(string longName) {

			Optional<Error> error = Validation.Validators.GameTextValidator.NameValidator_Length(longName);

			Assert.True(error.HasValue);
			Assert.Equal(error.Value, Validation.Data.GameValidationData.Name.Length.TooLongAdvisory);
		}

	}

	// TODO: probably right more tests, IDK if I should have one for each error level hard coded or have logic to test.

}