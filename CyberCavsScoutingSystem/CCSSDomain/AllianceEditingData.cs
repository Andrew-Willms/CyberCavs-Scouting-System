using System;
using System.Windows.Media;

using WPFUtilities;

namespace CCSSDomain;

public class AllianceEditingData {

	private AllianceEditingDataValidator Validator { get; }

	public AllianceEditingData(GameEditingData editingData) {

		Validator = new(editingData);

		Name = new(Validator.NameValidator, "");

		AllianceColor = new(Validator.ColorCovalidator,
			(nameof(Color.R), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, "0")),
			(nameof(Color.G), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, "0")),
			(nameof(Color.B), new StringInput<byte, ErrorSeverity>(Validator.ColorValueValidator, "0"))
		);
	}



	public StringInput<string, ErrorSeverity> Name { get; }

	public MultiStringInput<Color, ErrorSeverity> AllianceColor { get; }
}