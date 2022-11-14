using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CCSSDomain;
using GameMakerWpf.ApplicationManagement;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views.DataField;



public partial class SelectionDataFieldView : UserControl, INotifyPropertyChanged {

	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();



	private SingleInput<string, string, ErrorSeverity>? _SelectedOption;
	public SingleInput<string, string, ErrorSeverity>? SelectedOption {
		get => _SelectedOption;
		set {
			_SelectedOption = value;
			OnPropertyChanged(nameof(SelectedOption));
			OnPropertyChanged(nameof(RemoveButtonIsEnabled));
		}
	}

	public bool RemoveButtonIsEnabled => SelectedOption is not null;



	public SelectionDataFieldView() {

		DataContext = this;

		InitializeComponent();

		ApplicationManager.RegisterGameProjectChangeAction(GameProjectChanged);
	}


	public SelectionDataFieldEditor SelectionDataFieldEditor {
		get => (SelectionDataFieldEditor) GetValue(SelectionDataFieldEditorProperty);
		set => SetValue(SelectionDataFieldEditorProperty, value);
	}

	public static readonly DependencyProperty SelectionDataFieldEditorProperty = DependencyProperty.Register(
		name: nameof(SelectionDataFieldEditor),
		propertyType: typeof(SelectionDataFieldEditor),
		ownerType: typeof(SelectionDataFieldView),
		typeMetadata: new FrameworkPropertyMetadata(DependencyPropertiesChanged));

	private static void DependencyPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		
		if (d is not SelectionDataFieldView control) {
			return;
		}

		if (e.Property.Name == nameof(SelectionDataFieldEditor)) {
			control.SelectionDataFieldEditor = (e.NewValue as SelectionDataFieldEditor)!;
		}
	}



	private void AddButton_Click(object sender, RoutedEventArgs e) {
		
		SelectionDataFieldEditor.AddOption("Test");
	}

	private void RemoveButton_Click(object sender, RoutedEventArgs e) {

		if (SelectedOption is null) {
			throw new InvalidOperationException("You should not be able to press the remove button while there is no Option selected.");
		}

		Result<SelectionDataFieldEditor.RemoveOptionError> result = SelectionDataFieldEditor.RemoveOption(SelectedOption);

		switch (result.Resolve()) {
			
			case Success:
				return;

			case SelectionDataFieldEditor.RemoveOptionError { ErrorType: SelectionDataFieldEditor.RemoveOptionError.Types.OptionNotFound }:
				ErrorPresenter.DisplayError(ErrorData.RemoveOptionError.OptionNotFoundCaption, ErrorData.RemoveOptionError.OptionNotFoundMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	private void MoveUpButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}

	private void MoveDownButton_Click(object sender, RoutedEventArgs e) {
		throw new NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;
	
	private void OnPropertyChanged(string? propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void GameProjectChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}