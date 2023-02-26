using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Results;
using QRCoder;
using Version = CCSSDomain.GameSpecification.Version;

namespace ScoutingApp.Views; 



public partial class QrCodeDisplay : ContentPage {
	
	public QrCodeDisplay() {

		InitializeComponent();

		Version version = new() {
			MajorNumber = 1,
			MinorNumber = 0,
			PatchNumber = 0,
			Name = "Name",
			Description = "Description"
		};

		ReadOnlyList<Alliance> alliances = new List<Alliance> {
			new() { Name = "Blue", Color = Color.Blue },
			new() { Name = "Red", Color = Color.Red}
		}.ToReadOnly();

		ReadOnlyList<DataFieldSpec> dataFieldSpecs = new List<DataFieldSpec> {
			new TextDataFieldSpec { Name = "Comments" }
		}.ToReadOnly();

		ReadOnlyList<InputSpec> setupTabInputs = ReadOnlyList<InputSpec>.Empty;
		ReadOnlyList<InputSpec> autoTabInputs = ReadOnlyList<InputSpec>.Empty;
		ReadOnlyList<InputSpec> teleTabInputs = ReadOnlyList<InputSpec>.Empty;
		ReadOnlyList<InputSpec> endgameTabInputs = ReadOnlyList<InputSpec>.Empty;

		IResult<GameSpec> gameSpecResult = GameSpec.Create("Game Name", 2023, "Description", version, 3, 2,
			alliances, dataFieldSpecs, setupTabInputs, autoTabInputs, teleTabInputs, endgameTabInputs);

		GameSpec gameSpec = ((gameSpecResult as IResult<GameSpec>.Success)!).Value;

		MatchDataCollector matchDataCollector = new(gameSpec) {
			Event = new Event {
				Name = "Test Event",
				EventCode = "TEST",
				StartDate = DateTime.MinValue,
				EndDate = DateTime.MaxValue
			}.Optionalize(),

			MatchNumber = 0u.Optionalize(),
			ReplayNumber = 0u.Optionalize(),
			IsPlayoff = false.Optionalize(),
			TeamNumber = 0u.Optionalize(),
			Alliance = new Alliance { Name = "Blue", Color = Color.Blue }.Optionalize()
		};

		TextDataField commentsDataField = (TextDataField)matchDataCollector.DataFields.First(x => x.Name == "Comments");
		commentsDataField.Text = "Good robot";

		string theDataToEncode = matchDataCollector.ConvertDataToCsv();





		QRCodeGenerator generator = new();

		QRCodeData? qrCodeData = generator.CreateQrCode(theDataToEncode, QRCodeGenerator.ECCLevel.L);

		//QRCode qrCode = new(qrCodeData);
		//
		//Bitmap? graphic = qrCode.GetGraphic(20, Color.Black, Color.Aqua, null);
		//
		//graphic.Save(@"c:\Users\Andrew\Downloads\button.png", ImageFormat.Png);
	}

}