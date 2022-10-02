using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain;



public class AllianceEditingData {

	private GameEditingData EditingData { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }

	public MultiInput<Color, ErrorSeverity, byte, byte, byte> AllianceColor { get; }



	public AllianceEditingData(GameEditingData editingData, Alliance initialValues) {

		EditingData = editingData;

		Name = new(AllianceValidator.NameConverter, AllianceValidator.NameInverter, initialValues.Name,
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_EndsWithAlliance),
			new ValidationSet<string, ErrorSeverity>(AllianceValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.NameValidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);

		AllianceColor = new(AllianceValidator.ColorConverter, AllianceValidator.ColorInverter,

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter, initialValues.Color.R.ToString()),

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter, initialValues.Color.G.ToString()),

			new SingleInput<byte, string, ErrorSeverity>(AllianceValidator.ColorComponentConverter, 
				AllianceValidator.ColorComponentInverter, initialValues.Color.B.ToString()),

			new ValidationSet<Color, IEnumerable<AllianceEditingData>, ErrorSeverity>(
				AllianceValidator.ColorCovalidator_Uniqueness,
				() => EditingData.Alliances.Where(x => x != this),
				EditingData.AllianceNameChanged)
		);
	}

}