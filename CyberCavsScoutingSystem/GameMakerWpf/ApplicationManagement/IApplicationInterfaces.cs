using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;
using GameMakerWpf.Domain;
using Microsoft.Win32;
using UtilitiesLibrary;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;

namespace GameMakerWpf.ApplicationManagement;



public interface IGameMakerMainView
{

    public void Show();

}

public interface ISavePrompter
{

    public class SavePromptError : Error<SavePromptResult> { }

    public enum SavePromptResult
    {
        SaveAndContinue,
        ContinueWithoutSaving,
        CancelOperation
    }

    public SavePromptResult PromptSave();

}

public interface IGameSaver
{

    public class SaveError : Error<SaveError.Types>
    {

        public enum Types
        {
            ProjectHasNoSaveLocation,
            ProjectCouldNotBeSerialized,
            SaveLocationCouldNotBeWrittenTo
        }

    }

    public class SaveAsError : Error<SaveAsError.Types>
    {

        public enum Types
        {
            Aborted,
            SaveLocationIsInvalid
        }

    }

    public class OpenError : Error<OpenError.Types>
    {

        public enum Types
        {
            Aborted,
            SaveLocationCouldNotBeRead,
            SavedDataCouldNotBeConverted
        }

    }



    public bool ProjectHasSaveLocation { get; }

    public Result<SaveError> Save(GameEditingData gameEditingData);

    public Result<SaveAsError> SaveAs(GameEditingData gameEditingData);

    public Result<GameEditingData, OpenError> Open();

}



public class GameSavePrompter : ISavePrompter
{

    public ISavePrompter.SavePromptResult PromptSave()
    {

        const string text = "Proceed without saving? Unsaved changes will be lost. Yes to proceed, No to save and proceed, Cancel to back out.";
        const string caption = "Prompt SaveAndContinue";
        const MessageBoxButton button = MessageBoxButton.YesNoCancel;

        MessageBoxResult result = MessageBox.Show(text, caption, button);

        return result switch
        {
            MessageBoxResult.Yes => ISavePrompter.SavePromptResult.ContinueWithoutSaving,
            MessageBoxResult.No => ISavePrompter.SavePromptResult.SaveAndContinue,
            MessageBoxResult.Cancel => ISavePrompter.SavePromptResult.CancelOperation,
            _ => throw new ShouldMatchOtherCaseException()
        };
    }

}


public class GameProjectSaver : IGameSaver
{

    public bool ProjectHasSaveLocation => FilePath.HasValue;

    private Optional<string> FilePath = Optional.NoValue;

    public Result<IGameSaver.SaveError> Save(GameEditingData gameEditingData)
    {

        if (!FilePath.HasValue)
        {

            return new IGameSaver.SaveError
            {
                ErrorType = IGameSaver.SaveError.Types.ProjectHasNoSaveLocation
            };
        }

        string serializedProject;
        try
        {
            serializedProject = JsonSerializer.Serialize(gameEditingData, SavingSerializerDefaults);

        }
        catch
        {

            return new IGameSaver.SaveError
            {
                ErrorType = IGameSaver.SaveError.Types.ProjectCouldNotBeSerialized
            };
        }

        try
        {
            File.WriteAllText(FilePath.Value, serializedProject);

        }
        catch
        {

            return new IGameSaver.SaveError
            {
                ErrorType = IGameSaver.SaveError.Types.SaveLocationCouldNotBeWrittenTo
            };
        }

        return new Success();
    }

    public Result<IGameSaver.SaveAsError> SaveAs(GameEditingData gameEditingData)
    {

        SaveFileDialog saveFileDialog = SaveFileDialog;

        bool? proceed = saveFileDialog.ShowDialog();

        if (proceed is null or false)
        {
            return new IGameSaver.SaveAsError { ErrorType = IGameSaver.SaveAsError.Types.Aborted };
        }

        string filePath = saveFileDialog.FileName;
        string[] filePathPieces = filePath.Split("\\");
        string folderPath = string.Join("\\", filePathPieces[..^1]);

        if (!Directory.Exists(folderPath))
        {
            return new IGameSaver.SaveAsError { ErrorType = IGameSaver.SaveAsError.Types.SaveLocationIsInvalid };
        }

        FilePath = filePath.Optionalize();

        return new Success();
    }

    public Result<GameEditingData, IGameSaver.OpenError> Open()
    {

        OpenFileDialog openFileDialog = OpenFileDialog;

        bool? proceed = openFileDialog.ShowDialog();

        if (proceed is null or false)
        {
            return new IGameSaver.OpenError { ErrorType = IGameSaver.OpenError.Types.Aborted };
        }

        string filePath = openFileDialog.FileName;
        string serializedGameEditingData;

        try
        {
            serializedGameEditingData = File.ReadAllText(filePath);

        }
        catch
        {
            return new IGameSaver.OpenError { ErrorType = IGameSaver.OpenError.Types.SaveLocationCouldNotBeRead };
        }

        try
        {
            GameEditingData? newGameEditingData = JsonSerializer.Deserialize<GameEditingData>(serializedGameEditingData);
            return newGameEditingData ?? throw new NoNullAllowedException();

        }
        catch
        {
            return new IGameSaver.OpenError { ErrorType = IGameSaver.OpenError.Types.SavedDataCouldNotBeConverted };
        }
    }



    private static readonly JsonSerializerOptions SavingSerializerDefaults = new()
    {
        WriteIndented = true,
    };

    private static readonly OpenFileDialog OpenFileDialog = new()
    {
        Title = "Select a file to open.",
        Filter = "CCSS Game Project (*.cgp)|*.cgp"
    };

    private static readonly SaveFileDialog SaveFileDialog = new()
    {
        Title = "Select a file name and location for the project to be saved.",
        Filter = "CCSS Game Project (*.cgp)|*.cgp"
    };

}