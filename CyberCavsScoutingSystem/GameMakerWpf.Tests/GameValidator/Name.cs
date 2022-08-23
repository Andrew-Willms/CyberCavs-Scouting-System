using System.Collections.Generic;
using GameMakerWpf.Validation.Data;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using Xunit;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Tests.GameValidator;



public class Name {

	public class Converter {

		[Fact]
		public void NullNameShouldThrowException() {

			Assert.Throws<NullInputObjectInConverterException>(() => 
				GameTextValidator.NameConversionPair.Converter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= GameTextValidator.NameConversionPair.Converter(validName);

			Assert.True(optional.HasValue);
			Assert.Empty(errors);
			Assert.Equal(optional.Value, validName);
		}

	}

	public class Inverter {

		[Fact]
		public void NullNameShouldThrowException() {

			Assert.Throws<NullInputObjectInInverterException>(() => 
				GameTextValidator.NameConversionPair.Inverter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= GameTextValidator.NameConversionPair.Inverter(validName);

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

			Optional<Error> error = GameTextValidator.NameValidator_Length("");

			Assert.True(error.HasValue);
			Assert.Equal(error.Value, GameValidationData.Name.Length.TooShortError);
		}


		[Theory]
		[MemberData(nameof(LongNameShouldAdviseTestData))]
		public void LongNameShouldAdvise(string longName) {

			Optional<Error> error = GameTextValidator.NameValidator_Length(longName);

			Assert.True(error.HasValue);
			Assert.Equal(error.Value, GameValidationData.Name.Length.TooLongAdvisory);
		}

	}

	// TODO: probably right more tests, IDK if I should have one for each error level hard coded or have logic to test.

}