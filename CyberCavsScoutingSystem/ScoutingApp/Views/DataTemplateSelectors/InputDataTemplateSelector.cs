using System;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplateSelectors;



public class InputDataTemplateSelector : DataTemplateSelector {

	public DataTemplate TextDataFieldTemplate { get; set; } = null!;
	public DataTemplate IntegerDataFieldTemplate { get; set; } = null!;
	public DataTemplate SelectionDataFieldTemplate { get; set; } = null!;

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {

		return item switch {
			TextDataField => TextDataFieldTemplate,
			IntegerDataField => IntegerDataFieldTemplate,
			SelectionDataField => SelectionDataFieldTemplate,
			_ => throw new InvalidOperationException()
		};
	}

}