using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

using WPFUtilities;

namespace CCSSDomain;

public class AllianceEditingData : INotifyPropertyChanged {

	private AllianceEditingDataValidator Validator { get; }



	public AllianceEditingData() {

		Validator = new(this);

		Name = new(Validator.NameValidator, "");

		RedColorValue = new(Validator.ColorValueValidator, "0");
		GreenColorValue = new(Validator.ColorValueValidator, "0");
		BlueColorValue = new(Validator.ColorValueValidator, "0");
	}



	public StringInput<string, ErrorSeverity> Name { get; }
	public StringInput<int, ErrorSeverity> RedColorValue { get; }
	public StringInput<int, ErrorSeverity> GreenColorValue { get; }
	public StringInput<int, ErrorSeverity> BlueColorValue { get; }



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}