using System;
using System.Windows;
using System.Windows.Input;
using UtilitiesLibrary;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.AppManagement;



public partial class SavePrompter : Window, ISavePrompter {

	private Optional<ISavePrompter.SavePromptResult> SelectedOption { get; set; } = Optional.NoValue;



	public SavePrompter() {
		InitializeComponent();
	}

	private void Window_MouseDown(object sender, MouseButtonEventArgs e) {

		if (e.ChangedButton == MouseButton.Left) {
			DragMove();
		}
	}



	public ISavePrompter.SavePromptResult PromptSave() {

		ShowDialog();

		return SelectedOption.HasValue
			? SelectedOption.Value
			: ISavePrompter.SavePromptResult.Cancel;
	}

	private void SaveAndContinue_Clicked(object sender, EventArgs e) {
		SelectedOption = ISavePrompter.SavePromptResult.SaveAndContinue.Optionalize();
		Close();
	}

	private void ContinueWithoutSaving_Clicked(object sender, EventArgs e) {
		SelectedOption = ISavePrompter.SavePromptResult.ContinueWithoutSaving.Optionalize();
		Close();
	}

	private void Cancel_Clicked(object sender, EventArgs e) {
		SelectedOption = ISavePrompter.SavePromptResult.Cancel.Optionalize();
		Close();
	}

}