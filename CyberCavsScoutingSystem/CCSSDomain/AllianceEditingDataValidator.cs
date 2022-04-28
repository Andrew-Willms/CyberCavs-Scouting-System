﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Linq;

using WPFUtilities;

namespace CCSSDomain; 

public class AllianceEditingDataValidator {

	private GameEditingData EditingData { get; }

	private AllianceEditingData AllianceEditingData { get; }

	public AllianceEditingDataValidator(GameEditingData editingData, AllianceEditingData allianceEditingData) {

		EditingData = editingData;
		AllianceEditingData = allianceEditingData;
	}



	public (string, ReadOnlyCollection<ValidationError<ErrorSeverity>>) NameValidator(string inputString) {

		return (inputString, new List<ValidationError<ErrorSeverity>>().AsReadOnly());
	}

	public (byte, ReadOnlyCollection<ValidationError<ErrorSeverity>>) ColorValueValidator(string inputString) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		byte byteValue = 0;

		string invalidCharacters = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

		if (invalidCharacters.Length > 0) {
			newErrors.Add(new("Invalid Characters", ErrorSeverity.Error, "asdf"));

		} else {

			try {
				byteValue = byte.Parse(inputString);

			} catch (Exception ex) {

				// It really should be an overflow exception but check anyway.
				if (ex is OverflowException) {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a maximum of 255"));

				} else {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a minimum of 0"));;
				}
			}

		}

		return (byteValue, newErrors.AsReadOnly());
	}

	public (Color, ReadOnlyCollection<ValidationError<ErrorSeverity>>) ColorCovalidator
		(in ReadOnlyDictionary<string, IStringInput<ErrorSeverity>> inputComponents) {

		var redValueInput = inputComponents[nameof(Color.R)] as StringInput<byte, ErrorSeverity>;
		var greenValueInput = inputComponents[nameof(Color.G)] as StringInput<byte, ErrorSeverity>;
		var blueValueInput = inputComponents[nameof(Color.B)] as StringInput<byte, ErrorSeverity>;

		if (redValueInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.R)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (greenValueInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.G)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (blueValueInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.B)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		// Maybe check to make sure the red green and blue components are valid?

		Color color = Color.FromRgb(redValueInput.TargetObject, greenValueInput.TargetObject, blueValueInput.TargetObject);
		List<ValidationError<ErrorSeverity>> validationErrors = new();

		// TODO: compare color to other alliance colors to make sure two alliances don't have colors that are too similar.

		if (EditingData.Alliances != null) {

			foreach (AllianceEditingData allianceEditingData in EditingData.Alliances) {

				if (allianceEditingData == AllianceEditingData) {
					continue;
				}

				int colorDifference = ColorDifference(allianceEditingData.AllianceColor.TargetObject,
					AllianceEditingData.AllianceColor.TargetObject);

				switch (colorDifference) {

					case 0:
						validationErrors.Add(new("Colors Identical", ErrorSeverity.Warning,
							$"The color of this alliance are identical to that of the {allianceEditingData.Name.InputString}"));
						break;

					case < 10:
						validationErrors.Add(new("Colors Very Close", ErrorSeverity.Warning,
							$"The color of this alliance are very similar to that of the {allianceEditingData.Name.InputString}"));
						break;

					case < 30:
						validationErrors.Add(new("Colors Very Close", ErrorSeverity.Advisory,
							$"The color of this alliance are similar to that of the {allianceEditingData.Name.InputString}"));
						break;
				}
			}

		}

		return (color, validationErrors.AsReadOnly());
	}

	private int ColorDifference(Color color1, Color color2) {

		return Math.Abs(color1.R - color2.R) + Math.Abs(color1.G - color2.G) + Math.Abs(color1.B - color2.B);
	}


}