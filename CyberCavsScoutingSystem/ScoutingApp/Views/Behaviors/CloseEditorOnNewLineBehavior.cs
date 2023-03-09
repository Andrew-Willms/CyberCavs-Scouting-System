using Microsoft.Maui.Controls;
using System;
using System.Linq;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Behaviors; 



public class CloseEditorOnNewLineBehavior : Behavior<Editor>  {

	protected override void OnAttachedTo(Editor entry) {
		entry.TextChanged += OnEntryTextChanged;
		base.OnAttachedTo(entry);
	}

	protected override void OnDetachingFrom(Editor entry) {
		entry.TextChanged -= OnEntryTextChanged;
		base.OnDetachingFrom(entry);
	}

	private static void OnEntryTextChanged(object? sender, TextChangedEventArgs args) {

		if (sender is not Editor editor) {
			throw new InvalidOperationException();
		}

		if (string.IsNullOrEmpty(args.NewTextValue)) {
			return;
		}

		bool enterPressed = args.NewTextValue.Contains('\n');

		editor.Text = args.NewTextValue.Where(x => x is not '\n').CharArrayToString();

		if (!enterPressed) {
			return;
		}

		// Trick to hide keyboard
		editor.IsEnabled = false;
		editor.IsEnabled = true;
	}

}