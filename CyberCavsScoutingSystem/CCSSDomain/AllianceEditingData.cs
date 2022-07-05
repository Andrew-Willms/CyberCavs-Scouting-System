using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WPFUtilities.Validation;
using WPFUtilities.Validation.Inputs;

namespace CCSSDomain;

public class AllianceEditingData {

	private GameEditingData EditingData { get; }

	public AllianceEditingData(GameEditingData editingData) {

		EditingData = editingData;

		Name = new(AllianceEditingDataValidator.NameConversionPair, "",
			new ValidationSet<string, ErrorSeverity>(AllianceEditingDataValidator.NameValidator_EndsWithAlliance),
			new ValidationSet<string, ErrorSeverity>(AllianceEditingDataValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceEditingDataValidator.NameValidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this), EditingData.AllianceNameChanged)
		);

		AllianceColor = new(AllianceEditingDataValidator.ColorConverter, AllianceEditingDataValidator.ColorInverter,
			new SingleInput<byte, string, ErrorSeverity>(AllianceEditingDataValidator.ColorComponentConversionPair, "0"),
			new SingleInput<byte, string, ErrorSeverity>(AllianceEditingDataValidator.ColorComponentConversionPair, "0"),
			new SingleInput<byte, string, ErrorSeverity>(AllianceEditingDataValidator.ColorComponentConversionPair, "0"),
			new ValidationSet<Color, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceEditingDataValidator.ColorCovalidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this), EditingData.AllianceNameChanged)
		);
	}



	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }

}