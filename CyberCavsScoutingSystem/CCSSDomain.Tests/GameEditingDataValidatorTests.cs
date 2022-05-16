using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPFUtilities;
using Xunit;

namespace CCSSDomain.Tests; 

public class GameEditingDataValidatorTests {

	[Fact]
	public void NullNameShouldThrowException() {

		GameEditingData editingData = new();
		GameEditingDataValidator validator = new(editingData);

		Assert.Throws<ArgumentNullException>(() => validator.NameValidator(null));
	}

	[Fact]
	public void EmptyNameShouldFail() {

		GameEditingData editingData = new();
		GameEditingDataValidator validator = new(editingData);

		(string name, ReadOnlyCollection<ValidationError<ErrorSeverity>> errors) = validator.NameValidator("");

		Assert.Equal("", name);

		Assert.True(errors.Count == 1);

		Assert.True(errors[0].Severity == ErrorSeverity.Error);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[InlineData("test", "test")]
	public void ValidNameShouldEcho(string inputString, string expectedName) {

		GameEditingData editingData = new();
		GameEditingDataValidator validator = new(editingData);

		(string name, ReadOnlyCollection<ValidationError<ErrorSeverity>> errors) = validator.NameValidator(inputString);

		Assert.Equal(expectedName, name);
		Assert.True(errors.Count == 0);
	}

	[Theory]
	[MemberData(nameof(LongNameShouldAdviseTestData))]
	public void LongNameShouldAdvise(string inputString, string expectedName, ValidationError<ErrorSeverity> expectedError) {

		GameEditingData editingData = new();
		GameEditingDataValidator validator = new(editingData);

		(string name, ReadOnlyCollection<ValidationError<ErrorSeverity>> errors) = validator.NameValidator(inputString);

		Assert.Equal(expectedName, name);

		Assert.True(errors.Count == 1);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		//Assert.True(errors[0] == expectedError);
	}

	public static IEnumerable<object[]> LongNameShouldAdviseTestData() {

		yield return new object[] {
			"Some Really Long Name One Two Three Four Five Six Seven",
			"Some Really Long Name One Two Three Four Five Six Seven",
			new ValidationError<ErrorSeverity>("", ErrorSeverity.Advisory)
		};
	}
}