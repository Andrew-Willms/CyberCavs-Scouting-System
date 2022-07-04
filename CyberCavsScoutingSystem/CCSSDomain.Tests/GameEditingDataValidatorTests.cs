using System;
using System.Collections.Generic;
using WPFUtilities;
using WPFUtilities.Validation;
using Xunit;

namespace CCSSDomain.Tests;

public class GameEditingDataValidatorTests {

	[Fact]
	public void NullNameShouldThrowException() {

		Assert.Throws<ArgumentNullException>(() => GameEditingDataValidator.NameConverter(null));
	}

	[Fact]
	public void EmptyNameShouldFail() {

		ValidationError<ErrorSeverity>? error = GameEditingDataValidator.NameValidator_Length("");

		Assert.True(error is not null);

		Assert.True(error!.Severity == ErrorSeverity.Error);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[InlineData("test")]
	public void VeryShortNameShouldWarn(string name) {

		ValidationError<ErrorSeverity>? error = GameEditingDataValidator.NameValidator_Length(name);

		Assert.True(error is not null);

		Assert.True(error!.Severity == ErrorSeverity.Warning);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[InlineData("test")]
	public void ShortNameShouldAdvise(string name) {

		ValidationError<ErrorSeverity>? error = GameEditingDataValidator.NameValidator_Length(name);

		Assert.True(error is not null);

		Assert.True(error!.Severity == ErrorSeverity.Advisory);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[MemberData(nameof(LongNameShouldAdviseTestData))]
	public void LongNameShouldAdvise(string name) {

		ValidationError<ErrorSeverity>? error = GameEditingDataValidator.NameValidator_Length(name);

		Assert.True(error is not null);

		Assert.True(error!.Severity == ErrorSeverity.Advisory);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[InlineData("test")]
	public void VeryLongNameShouldWard(string name) {

		ValidationError<ErrorSeverity>? error = GameEditingDataValidator.NameValidator_Length(name);

		Assert.True(error is not null);

		Assert.True(error!.Severity == ErrorSeverity.Warning);

		//TODO: once there is a resource to get standardized errors from assert that the error is the right one.
		// Look at "LongNameShouldWarn" for how to set this up
		// I should have some way of getting localized text like error messages before I test this.
		//Assert.True(errors[0].Name == "");
		//Assert.True(errors[0].Description == "");
	}

	[Theory]
	[InlineData("test", "test")]
	public void ValidNameShouldEcho(string inputString, string expectedName) {

		GameEditingData editingData = GameEditingData.GetDefaultEditingData();

		(string name, ReadOnlyList<ValidationError<ErrorSeverity>> errors) = GameEditingDataValidator.NameConverter(inputString);

		Assert.Equal(expectedName, name);
		Assert.True(errors.Count == 0);
	}

	public static IEnumerable<object[]> LongNameShouldAdviseTestData() {

		yield return new object[] {
			"Some Really Long Name One Two Three Four Five Six Seven",
			"Some Really Long Name One Two Three Four Five Six Seven Eight",
		};
	}
}