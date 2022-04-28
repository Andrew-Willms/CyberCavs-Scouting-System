using System;
using System.Windows.Media;

using WPFUtilities;

namespace CCSSDomain;

public class AllianceEditingData {

	private AllianceEditingDataValidator Validator { get; }

	public AllianceEditingData(GameEditingData editingData, Color color)
		: this(editingData, color.R.ToString(), color.G.ToString(), color.B.ToString()) { }

	public AllianceEditingData(GameEditingData editingData, string redColorValue = "0", string greenColorValue = "0", string blueColorValue = "0") {

		Validator = new(editingData);

		Name = new(Validator.NameValidator, "");

		AllianceColor = new(Validator.ColorCovalidator,
			(nameof(Color.R), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, redColorValue)),
			(nameof(Color.G), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, greenColorValue)),
			(nameof(Color.B), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, blueColorValue))
		);
	}



	public StringInput<string, ErrorSeverity> Name { get; }

	public MultiStringInput<Color, ErrorSeverity> AllianceColor { get; }
}