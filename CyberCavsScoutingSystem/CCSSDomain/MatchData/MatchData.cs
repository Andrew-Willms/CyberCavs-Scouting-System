//using System;
//using CCSSDomain.DataCollectors;
//using CCSSDomain.GameSpecification;
//using UtilitiesLibrary.Collections;
//using UtilitiesLibrary.Results;

//namespace CCSSDomain.MatchData;



//public class MatchData {

//	public GameSpec GameSpecification { get; }

//	public required Event Event { get; init; }

//	public required uint MatchNumber { get; init; }
//	public required uint ReplayNumber { get; init; }
//	public required bool IsPlayoff { get; init; }

//    public required uint TeamNumber { get; init; }

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

//			if (gameSpecification.DataFields[i].GetType() != dataFieldResults[i].GetType() ||
//                gameSpecification.DataFields[i].Name != dataFieldResults[i].Name) {

//				return new IResult<MatchData>.Error("The DataFields do not match.");
//			}

//            if (gameSpecification.DataFields[i] is SelectionDataField selectionDataField) {

//            }
//        }


//	}

//}