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

			Assert.Throws<NullInputObjectInConverterException>(() => GameTextValidator.NameConverter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= GameTextValidator.NameConverter(validName);

			Assert.True(optional.HasValue);
			Assert.Empty(errors);
			Assert.Equal(optional.Value, validName);
		}

	}

	public class Inverter {

		[Fact]
		public void NullNameShouldThrowException() {

			Assert.Throws<NullInputObjectInInverterException>(() => 
				GameTextValidator.NameInverter(null!));
		}

		[Theory]
		[InlineData("test")]
		public void ValidNameShouldEcho(string validName) {

			(Optional<string> optional, ReadOnlyList<Error> errors)
				= GameTextValidator.NameInverter(validName);

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

			ReadOnlyList<Error> errors = GameTextValidator.NameValidator_Length("");

			Assert.True(errors.Count == 1);
			Assert.Equal(errors[0], GameValidationData.Name.Length.TooShortError);
		}


		[Theory]
		[MemberData(nameof(LongNameShouldAdviseTestData))]
		public void LongNameShouldAdvise(string longName) {

			ReadOnlyList<Error> errors = GameTextValidator.NameValidator_Length(longName);

			Assert.True(errors.Count == 1);
			Assert.Equal(errors[0], GameValidationData.Name.Length.TooLongAdvisory);
		}

	}

	// TODO: probably right more tests, IDK if I should have one for each error level hard coded or have logic to test.

}