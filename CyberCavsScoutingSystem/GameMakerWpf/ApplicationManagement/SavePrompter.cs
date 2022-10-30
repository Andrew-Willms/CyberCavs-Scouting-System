using System.ComponentModel;
using System.Windows;
using UtilitiesLibrary.Validation;

namespace GameMakerWpf.ApplicationManagement; 



public class SavePrompter : ISavePrompter {
	
	public ISavePrompter.SavePromptResult PromptSave() {

		const string text = "Proceed without saving? Unsaved changes will be lost. Yes to proceed, No to save and proceed, Cancel to back out.";
		const string caption = "Prompt SaveAndContinue";
		const MessageBoxButton button = MessageBoxButton.YesNoCancel;

		MessageBoxResult result = MessageBox.Show(text, caption, button);

		return result switch {
			MessageBoxResult.Yes => ISavePrompter.SavePromptResult.ContinueWithoutSaving,
			MessageBoxResult.No => ISavePrompter.SavePromptResult.SaveAndContinue,
			MessageBoxResult.Cancel => ISavePrompter.SavePromptResult.CancelOperation,
			MessageBoxResult.None => throw new ShouldMatchOtherCaseException(),
			MessageBoxResult.OK => throw new ShouldMatchOtherCaseException(),
			_ => throw new InvalidEnumArgumentException()
		};
	}

}