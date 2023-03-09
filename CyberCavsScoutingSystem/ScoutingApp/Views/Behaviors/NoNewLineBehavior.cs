using Microsoft.Maui.Controls;
using System;
using System.Linq;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Behaviors; 



public class NoNewLineBehavior : Behavior<Editor>  {

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

		int cursorPosition = editor.CursorPosition;

		editor.Text = args.NewTextValue.Where(x => x is not '\n').CharArrayToString();

		editor.CursorPosition = int.Max(cursorPosition, editor.Text.Length);
	}

}