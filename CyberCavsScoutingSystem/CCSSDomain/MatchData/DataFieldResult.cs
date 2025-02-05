using System.Linq;
using CCSSDomain.GameSpecification;
using ExhaustiveMatching;
using UtilitiesLibrary.Results;

namespace CCSSDomain.MatchData;



[Closed(typeof(BooleanDataFieldResult), typeof(TextDataFieldResult), typeof(IntegerDataFieldResult), typeof(SelectionDataFieldResult))]
public abstract class DataFieldResult;

public class BooleanDataFieldResult : DataFieldResult {

	public bool Value { get; private init; }

	public static IResult<BooleanDataFieldResult> Create(bool value) {

		return new IResult<BooleanDataFieldResult>.Success(new () { Value = value });
	}

}

public class TextDataFieldResult : DataFieldResult {

	public string Text { get; }

	private TextDataFieldResult(string text) {
		Text = text;
	}

	public static IResult<TextDataFieldResult> Create(TextDataFieldSpec dataFieldSpec, string text) {

		if (dataFieldSpec.MustNotBeEmpty && string.IsNullOrEmpty(text)) {
			return new IResult<TextDataFieldResult>.Error();
		}

		return new IResult<TextDataFieldResult>.Success(new(text));
	}

}

public class IntegerDataFieldResult : DataFieldResult {

	public int Value { get; }

	private IntegerDataFieldResult(int value)  {
		Value = value;
	}

	public static IResult<IntegerDataFieldResult> Create(IntegerDataFieldSpec dataFieldSpec, int value) {

		if (value > dataFieldSpec.MaxValue) {
			return new IResult<IntegerDataFieldResult>.Error();
		}

		if (value < dataFieldSpec.MinValue) {
			return new IResult<IntegerDataFieldResult>.Error();
		}

		return new IResult<IntegerDataFieldResult>.Success { Value = new(value) };
	}

}

public class SelectionDataFieldResult : DataFieldResult {

	public string SelectedOption { get; }

	private SelectionDataFieldResult(string selectedOption) {
		SelectedOption = selectedOption;
	}

	public static IResult<SelectionDataFieldResult> Create(SelectionDataFieldSpec dataFieldSpec, string selection) {

		if (!dataFieldSpec.OptionNames.Contains(selection)) {
			return new IResult<SelectionDataFieldResult>.Error();
		}

		return new IResult<SelectionDataFieldResult>.Success { Value = new(selection) };
	}

}