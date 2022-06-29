using System.Collections.Generic;
using System.Windows.Media;
using WPFUtilities.Validation;

namespace CCSSDomain;

public class AllianceEditingData {

	private GameEditingData EditingData { get; }

	public AllianceEditingData(GameEditingData editingData) {

		EditingData = editingData;

		Name = new(AllianceEditingDataValidator.NameConverter, "", new(EditingData.AllianceNameChanged),
			new ValidationSet<string, ErrorSeverity>(AllianceEditingDataValidator.NameValidator_EndsWithAlliance),
			new ValidationSet<string, ErrorSeverity>(AllianceEditingDataValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<AllianceEditingData>, ErrorSeverity>(AllianceEditingDataValidator.NameValidator_Duplicate,
				() => EditingData.Alliances, EditingData.AllianceNameChanged)
		);

		AllianceColor = new(AllianceEditingDataValidator.ColorCovalidator,
			(nameof(Color.R), new StringInput<byte, ErrorSeverity>(AllianceEditingDataValidator.ColorValueValidator, "0")),
			(nameof(Color.G), new StringInput<byte, ErrorSeverity>(AllianceEditingDataValidator.ColorValueValidator, "0")),
			(nameof(Color.B), new StringInput<byte, ErrorSeverity>(AllianceEditingDataValidator.ColorValueValidator, "0"))
		);
	}



	public StringInput<string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity> AllianceColor { get; }

}