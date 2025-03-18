using System.Collections;
using System.Drawing;
using CCSSDomain.Data;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using MatchType = CCSSDomain.Data.MatchType;

namespace CcssDomain.Tests.Serialization;



public class SampleData : IEnumerable<object[]> {

	private static readonly GameSpec GameSpec = (GameSpec.Create(
		name: "ReefScape",
		year: 2025,
		description: "",
		version: new(1, 0, 0),
		robotsPerAlliance: 3u,
		alliancesPerMatch: 2u,
		alliances: new List<AllianceColor> {
			new() { Color = Color.Red, Name = "Red Alliance" },
			new() { Color = Color.Blue, Name = "Blue Alliance" }
		}.ToReadOnly(),
		dataFields: new List<DataFieldSpec> {
			new IntegerDataFieldSpec { Name = "Auto L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Auto L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Auto L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Auto L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Auto Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
			new IntegerDataFieldSpec { Name = "Auto Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
			new IntegerDataFieldSpec { Name = "Tele L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Tele L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Tele L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Tele L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
			new IntegerDataFieldSpec { Name = "Tele Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
			new IntegerDataFieldSpec { Name = "Tele Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
			new SelectionDataFieldSpec {
				Name = "Climb",
				Options = new List<string> { "None", "Deep", "Shallow" }.ToReadOnly(),
				InitialValue = "None",
				RequiresValue = true
			},
			new SelectionDataFieldSpec {
				Name = "Disconnected",
				Options = new List<string> { "None of match", "Some of match", "Most of match", "All of match" }.ToReadOnly(),
				InitialValue = "None of match",
				RequiresValue = true
			},
			new SelectionDataFieldSpec {
				Name = "Defense",
				Options = new List<string> { "N/A", "1", "2", "3", "4", "5" }.ToReadOnly(),
				InitialValue = "N/A",
				RequiresValue = true
			},
			new TextDataFieldSpec { Name = "Comments", InitialValue = "", MustNotBeEmpty = true, MustNotBeInitialValue = false }
		}.ToReadOnly(),
		setupTabInputs: new List<InputSpec>().ToReadOnly(),
		autoTabInputs: new List<InputSpec> {
			new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
			new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
			new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
			new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" }
		}.ToReadOnly(),
		teleTabInputs: new List<InputSpec> {
			new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
			new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
			new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
			new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" }
		}.ToReadOnly(),
		endgameTabInputs: new List<InputSpec> {
			new() { DataFieldName = "Climb", Label = "Climb" },
			new() { DataFieldName = "Disconnected", Label = "Disconnected" },
			new() { DataFieldName = "Defense", Label = "Defense Effectiveness" },
			new() { DataFieldName = "Comments", Label = "Comments" },
		}.ToReadOnly()) as IResult<GameSpec>.Success)!.Value;

	private readonly List<object[]> Data = [

		[ new MatchData(
			errorContext: null!,
			gameSpecification: GameSpec, 
			eventCode: "",
			eventSchedule: null,
			scoutName: "",
			match: new() { MatchNumber = 1, Type = MatchType.Qualification, ReplayNumber = 0 },
			teamNumber: 0,
			allianceIndex: 0,
			startTime: DateTime.Now,
			endTime: DateTime.Now,
			dataFieldValues: new object[] {
				0,0,0,0,0,0,0,0,0,0,0,0,"None","None of match","N/A","Comments"
			}.ToReadOnly()
			) 
		],

	];

	public IEnumerator<object[]> GetEnumerator() {
		return Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

}