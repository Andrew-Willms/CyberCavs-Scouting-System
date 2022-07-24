using System.Collections.Generic;
using WPFUtilities;
using WPFUtilities.Validation;
using WPFUtilities.Validation.Errors;
using Xunit;

namespace CCSSDomain.Tests.GameValidator;



public class Name {

	[Fact]
	public void NullNameShouldThrowException() {

		Assert.Throws<NullInputObjectInConverterException>(() => 
			Validation.Validators.GameTextValidator.NameConversionPair.Converter(null!));
	}

	[Fact]
	public void EmptyNameShouldError() {

		(Optional<string> optional, ReadOnlyList<ValidationError<ErrorSeverity>> errors)
			= Validation.Validators.GameTextValidator.NameConversionPair.Converter("");

		Assert.False(optional.HasValue);
		Assert.True(errors.Count == 1);
		Assert.Equal(errors[0], Validation.Data.GameValidationData.Name.Length.TooLongError);
	}

	[Theory]
	[InlineData("test")]
	public void ValidNameShouldEcho(string validName) {

		(Optional<string> optional, ReadOnlyList<ValidationError<ErrorSeverity>> errors)
			= Validation.Validators.GameTextValidator.NameConversionPair.Converter(validName);

		Assert.True(optional.HasValue);
		Assert.Empty(errors);
		Assert.Equal(optional.Value, validName);
	}



	// TODO: probably right more tests, IDK if I should have one for each error level hard coded or have logic to test.



	[Theory]
	[MemberData(nameof(LongNameShouldAdviseTestData))]
	public void LongNameShouldAdvise(string longName) {

		(Optional<string> optional, ReadOnlyList<ValidationError<ErrorSeverity>> errors)
			= Validation.Validators.GameTextValidator.NameConversionPair.Converter(longName);

		Assert.True(optional.HasValue);
		Assert.Equal(optional.Value, longName);
		Assert.True(errors.Count == 1);
		Assert.Equal(errors[0], Validation.Data.GameValidationData.Name.Length.TooLongAdvisory);
	}

	public static IEnumerable<object[]> LongNameShouldAdviseTestData() {

		yield return new object[] {
			"Some Really Long Name One Two Three Four Five Six Seven",
			"Some Really Long Name One Two Three Four Five Six Seven Eight",
		};
	}

}