//using System.Linq;
//using CCSSDomain.GameSpecification;
//using UtilitiesLibrary.Results;

//namespace CCSSDomain.MatchData;



//public abstract class DataFieldResult {

//	public DataFieldSpec DataFieldSpec { get; }

//	public string Name => DataFieldSpec.Name;

//	protected DataFieldResult(DataFieldSpec dataFieldSpec) {
//		DataFieldSpec = dataFieldSpec;
//	}

//}

//public class TextDataFieldResult : DataFieldResult {

//	public string Text { get; }

//	private TextDataFieldResult(TextDataFieldSpec dataFieldSpec, string text) : base(dataFieldSpec) {
//		Text = text;
//	}

//	public static TextDataFieldResult Create(TextDataFieldSpec dataFieldSpec, string text) {

//		return new(dataFieldSpec, text);
//	}

//}

//public class IntegerDataFieldResult : DataFieldResult {

//	public int Value { get; }

//	private IntegerDataFieldResult(IntegerDataFieldSpec dataFieldSpec, int value) : base(dataFieldSpec) {
//		Value = value;
//	}

//	public static IResult<IntegerDataFieldResult> Create(IntegerDataFieldSpec dataFieldSpec, int value) {

//		if (value > dataFieldSpec.MaxValue) {
//			return new IResult<IntegerDataFieldResult>.Error();
//		}

//		if (value < dataFieldSpec.MinValue) {
//			return new IResult<IntegerDataFieldResult>.Error();
//		}

//		return new IResult<IntegerDataFieldResult>.Success { Value = new(dataFieldSpec, value) };
//	}

//}

//public class SelectionDataFieldResult : DataFieldResult {

//	public string Selection { get; }

//	private SelectionDataFieldResult(SelectionDataFieldSpec dataFieldSpec, string selection) : base(dataFieldSpec) {
//		Selection = selection;
//	}

//	public static IResult<SelectionDataFieldResult> Create(SelectionDataFieldSpec dataFieldSpec, string selection) {

//		if (!dataFieldSpec.OptionNames.Contains(selection)) {
//			return new IResult<SelectionDataFieldResult>.Error();
//		}

//		return new IResult<SelectionDataFieldResult>.Success { Value = new(dataFieldSpec, selection) };
//	}

//}