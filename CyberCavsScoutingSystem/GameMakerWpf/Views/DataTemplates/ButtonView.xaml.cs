using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CCSSDomain;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Views.DataTemplates;



public partial class ButtonView : UserControl, INotifyPropertyChanged {

	public SingleInput<string, string, ErrorSeverity>? DataFieldName => Editor?.DataFieldName;
	public SingleInput<string, string, ErrorSeverity>? ButtonText => Editor?.ButtonText;
	public SingleInput<int, string, ErrorSeverity>? IncrementAmount => Editor?.IncrementAmount;
	public SingleInput<double, string, ErrorSeverity>? XPosition => Editor?.XPosition;
	public SingleInput<double, string, ErrorSeverity>? YPosition => Editor?.YPosition;
	public SingleInput<double, string, ErrorSeverity>? ButtonWidth => Editor?.Width;
	public SingleInput<double, string, ErrorSeverity>? ButtonHeight => Editor?.Height;



	public ButtonView() {

		DataContext = this;

		InitializeComponent();
	}



	// This should be nullable since the ButtonView initializes before the Editor is set to a value.
	public ButtonEditor? Editor {
		get => (ButtonEditor)GetValue(EditorProperty);
		set => SetValue(EditorProperty, value);
	}

	public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
		name: nameof(Editor),
		propertyType: typeof(ButtonEditor),
		ownerType: typeof(ButtonView),
		typeMetadata: new FrameworkPropertyMetadata(DependencyPropertiesChanged));

	private static void DependencyPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		
		if (d is not ButtonView control) {
			return;
		}

		if (e.Property.Name == nameof(Editor)) {
			control.Editor = (e.NewValue as ButtonEditor)!;
		}

		control.EditorChanged();
	}



	public event PropertyChangedEventHandler? PropertyChanged;
	
	private void EditorChanged() {
		PropertyChanged?.Invoke(this, new(""));
	}

}