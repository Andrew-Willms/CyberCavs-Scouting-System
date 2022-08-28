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

	public AllianceEditingData(GameEditingData editingData, string allianceName, Color allianceColor) {

		EditingData = editingData;

		Name = new(AllianceValidator.NameConverter, AllianceValidator.NameInverter, allianceName,
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_EndsWithAlliance),
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.NameValidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);

		AllianceColor = new(AllianceValidator.ColorConverter, AllianceValidator.ColorInverter,

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter,allianceColor.R.ToString()),

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter, allianceColor.G.ToString()),

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter, allianceColor.B.ToString()),

			new ValidationSet<Color, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.ColorCovalidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);
	}



	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }

}