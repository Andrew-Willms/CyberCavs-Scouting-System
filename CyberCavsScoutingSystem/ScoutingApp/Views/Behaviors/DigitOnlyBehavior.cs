﻿using System;
using System.Linq;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Behaviors; 



public class DigitOnlyBehavior : Behavior<Entry> {

	protected override void OnAttachedTo(Entry entry) {
		entry.TextChanged += OnEntryTextChanged;
		base.OnAttachedTo(entry);
	}

	protected override void OnDetachingFrom(Entry entry) {
		entry.TextChanged -= OnEntryTextChanged;
		base.OnDetachingFrom(entry);
	}

	private static void OnEntryTextChanged(object? sender, TextChangedEventArgs args) {

		if (sender is not Entry entry) {
			throw new InvalidOperationException();
		}

		if (string.IsNullOrEmpty(args.NewTextValue)) {
			return;
		}

		entry.Text = args.NewTextValue.Where(char.IsDigit).CharArrayToString();
	}

}