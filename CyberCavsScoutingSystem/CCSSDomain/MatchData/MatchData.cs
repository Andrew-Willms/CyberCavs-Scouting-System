//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using CCSSDomain.GameSpecification;
//using UtilitiesLibrary.Collections;
//using UtilitiesLibrary.Results;

//namespace CCSSDomain.MatchData;



//public class MatchData {

//	public GameSpec GameSpecification { get; }

//	public required Event Event { get; init; }

//	public required Match Match { get; init; }

//	public uint ReplayNumber { get; init; } = 0;

//	public required uint TeamNumber { get; init; }

//	public required Alliance Alliance { get; init; }

//	public required DateTime Time { get; init; }

//	public ReadOnlyList<DataFieldResult> DataFields { get; }



//	public MatchData(GameSpec gameSpecification, ReadOnlyList<DataFieldResult> dataFieldResults) {

//		//TODO: validate that the DataFields passed in match up with the DataFields in the GameSpecification

//		GameSpecification = gameSpecification;
//		DataFields = dataFieldResults;








//	}

//	public static IResult<MatchData> Create(GameSpec gameSpecification, ReadOnlyList<DataFieldResult> dataFieldResults) {

//		if (gameSpecification.DataFields.Count != dataFieldResults.Count) {
//			return new IResult<MatchData>.Error("The DataFields do not match.");
//		}

//		for (int i = 0; i < gameSpecification.DataFields.Count; i++) {

//			if (dataFieldResults[i].DataFieldSpec != gameSpecification.DataFields[i]) {
//				return new IResult<MatchData>.Error();
//			}

//			if (gameSpecification.DataFields[i].GetType() != dataFieldResults[i].GetType()) {
//				return new IResult<MatchData>.Error("The DataFields do not match.");
//			}

//			if (gameSpecification.DataFields[i].Name != dataFieldResults[i].Name) {
//				return new IResult<MatchData>.Error("The DataFields do not match.");
//			}
//		}


//	}



//	public string GetCsvHeaders() {

//		StringBuilder columnHeaders = new(
//			$"{nameof(Event).ToCsvFriendly()}," +
//			$"{nameof(Match).ToCsvFriendly()}," +
//			$"{nameof(ReplayNumber).ToCsvFriendly()}," +
//			$"{nameof(TeamNumber).ToCsvFriendly()}," +
//			$"{nameof(Alliance).ToCsvFriendly()}," +
//			$"{nameof(Time).ToCsvFriendly()},"
//		);

//		GameSpecification.DataFields.Foreach(x => columnHeaders.Append($"{x.Name.ToCsvFriendly()},"));

//		return columnHeaders.ToString();
//	}

//	public string ConvertDataToCsv() {

//		StringBuilder matchData = new(
//			$"{Event.Name.ToCsvFriendly()}," +
//			$"{Match.ToString().ToCsvFriendly()}," +
//			$"{ReplayNumber.ToString().ToCsvFriendly()}," +
//			$"{TeamNumber.ToString().ToCsvFriendly()}," +
//			$"{Alliance.Name.ToCsvFriendly()}," +
//			$"{Time.ToString(CultureInfo.InvariantCulture).ToCsvFriendly()},");

//		foreach (DataFieldSpec dataField in GameSpecification.DataFields) {

//			switch (dataField) {

//				case TextDataFieldSpec textDataField:

//					IEnumerable<TextDataFieldResult> textDataFieldEnumerable = DataFields
//						.Where(x => x is TextDataFieldResult && x.Name == textDataField.Name)
//						.Select(x => x as TextDataFieldResult ?? throw new UnreachableException())
//						.ToArray();

//					if (textDataFieldEnumerable.OnlyOne()) {

//						TextDataFieldResult textResult = textDataFieldEnumerable.First();
//						matchData.Append($"{textResult.Text.ToCsvFriendly()},");
//					}

//					break;

//				case IntegerDataFieldSpec integerDataField:

//					IEnumerable<IntegerDataFieldResult> integerDataFieldEnumerable =
//						DataFields.Where(x => x is IntegerDataFieldResult && x.Name == integerDataField.Name)
//						.Select(x => x as IntegerDataFieldResult ?? throw new UnreachableException())
//						.ToArray();

//					if (integerDataFieldEnumerable.OnlyOne()) {

//						IntegerDataFieldResult integerResult = integerDataFieldEnumerable.First();
//						matchData.Append($"{integerResult.Value.ToString().ToCsvFriendly()},");
//					}

//					break;

//				case SelectionDataFieldSpec selectionDataField:

//					IEnumerable<SelectionDataFieldResult> selectionDataFieldEnumerable =
//						DataFields.Where(x => x is SelectionDataFieldResult && x.Name == selectionDataField.Name)
//							.Select(x => x as SelectionDataFieldResult ?? throw new UnreachableException())
//							.ToArray();

//					if (selectionDataFieldEnumerable.OnlyOne()) {

//						SelectionDataFieldResult selectionResult = selectionDataFieldEnumerable.First();
//						matchData.Append($"{selectionResult.Selection.ToCsvFriendly()},");
//					}

//					break;

//				default:
//					throw new UnreachableException();
//			}
//		}

//		return matchData.ToString();
//	}

//}