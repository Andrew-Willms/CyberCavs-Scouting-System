using System;
using System.Windows.Media;
using CCSSDomain.Models;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.Domain.DomainData;



public static class DefaultEditingDataValues
{

    private static readonly Alliance DefaultRedAlliance = new()
    {
        Name = "Red Alliance",
        Color = Colors.Red
    };

    private static readonly Alliance DefaultBlueAlliance = new()
    {
        Name = "Blue Alliance",
        Color = Colors.Blue
    };



    public static readonly TextDataField DefaultTextDataField = new()
    {
        Name = "New Data Field"
    };

    public static readonly SelectionDataField DefaultSelectionDataField = new()
    {
        Name = "New Data Field",
        OptionNames = ReadOnlyList<string>.Empty
    };

    public static readonly IntegerDataField DefaultIntegerDataField = new()
    {
        Name = "New Data Field"
    };

    public static readonly DataField DefaultDataField = DefaultTextDataField;



    public static GameEditingData DefaultEditingData
    {

        get
        {
            Game initialValues = new()
            {
                Name = GameNameGenerator.GetRandomGameName(),
                Year = DateTime.Now.Year,
                RobotsPerAlliance = 3,
                AlliancesPerMatch = 2,
                Alliances = new ReadOnlyList<Alliance>(),
                DataFields = new ReadOnlyList<DataField>()
            };

            GameEditingData gameEditingData = new(initialValues);

            gameEditingData.AddAlliance(new(gameEditingData, DefaultRedAlliance));
            gameEditingData.AddAlliance(new(gameEditingData, DefaultBlueAlliance));

            return gameEditingData;
        }
    }

    public static AllianceEditingData GetNewAlliance(GameEditingData gameEditingData)
    {

        Alliance initialValues = AllianceGenerator.GenerateAlliance(gameEditingData.Alliances.SelectIfHasValue(x => x.Name.OutputObject));
        return new(gameEditingData, initialValues);
    }

    public static GeneralDataFieldEditingData GetNewDataField(GameEditingData gameEditingData)
    {

        return new(gameEditingData, DefaultDataField);
    }



    public static void AddGeneratedAlliance(this GameEditingData gameEditingData)
    {

        gameEditingData.AddAlliance(GetNewAlliance(gameEditingData));
    }

    public static void AddGeneratedDataField(this GameEditingData gameEditingData)
    {

        gameEditingData.AddDataField(GetNewDataField(gameEditingData));
    }

}