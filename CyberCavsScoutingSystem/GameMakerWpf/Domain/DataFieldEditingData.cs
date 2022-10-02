using System.Linq;
using System.Collections.Generic;
using CCSSDomain;
using CCSSDomain.Models;
using GameMakerWpf.Validation.Validators;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain; 



public class DataFieldEditingData {

	private GameEditingData EditingData { get; }

	public SingleInput<string, string, ErrorSeverity> Name { get; }



	public DataFieldEditingData(GameEditingData editingData, DataField initialValues) {

		EditingData = editingData;

		Name = new(DataFieldValidator.NameConverter, DataFieldValidator.NameInverter, initialValues.Name,
			new ValidationSet<string, ErrorSeverity>(DataFieldValidator.NameValidator_Length),
			new ValidationSet<string, IEnumerable<DataFieldEditingData>, ErrorSeverity>(
				DataFieldValidator.NameValidator_Uniqueness,
				() => EditingData.DataFields.Where(x => x != this),
				EditingData.DataFieldNameChanged)
		);

	}

}