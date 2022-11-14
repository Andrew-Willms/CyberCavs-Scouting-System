﻿using System;
using System.Windows;
using System.Windows.Controls;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;

namespace GameMakerWpf.Views.DataField;



public partial class IntegerDataFieldView : UserControl {

	private IntegerDataFieldEditor DataContextAsIntegerDataFieldEditor =>
		DataContext as IntegerDataFieldEditor 
		?? throw new InvalidOperationException($"The {nameof(DataContext)} for this class must be of type {typeof(IntegerDataFieldEditor)}");

	public IntegerDataFieldView() {
		InitializeComponent();
	}

	private void MinButton_Click(object sender, RoutedEventArgs e) {

		DataContextAsIntegerDataFieldEditor.MinValue.InputObject = int.MinValue.ToString();
	}

	private void MaxButton_Click(object sender, RoutedEventArgs e) {

		DataContextAsIntegerDataFieldEditor.MaxValue.InputObject = int.MaxValue.ToString();
	}

}
