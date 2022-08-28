using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CCSSDomain;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.EditingData;



public class AllianceEditingData {

	private GameEditingData EditingData { get; }

	public AllianceEditingData(GameEditingData editingData) {

		EditingData = editingData;

		Name = new(AllianceValidator.NameConverter, AllianceValidator.NameInverter, "",
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_EndsWithAlliance),
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.NameValidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);

		AllianceColor = new(AllianceValidator.ColorConverter, AllianceValidator.ColorInverter,
			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, AllianceValidator.ColorComponentInverter, "0"),
			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, AllianceValidator.ColorComponentInverter, "0"),
			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, AllianceValidator.ColorComponentInverter, "0"),
			new ValidationSet<Color, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.ColorCovalidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);
	}



	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }

}