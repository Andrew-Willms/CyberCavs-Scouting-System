using System.Diagnostics;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.DataTemplateSelectors;



public class InputDataTemplateSelector : DataTemplateSelector {

	public DataTemplate TextDataFieldTemplate { get; set; } = null!;
	public DataTemplate IntegerDataFieldTemplate { get; set; } = null!;
	public DataTemplate SelectionDataFieldTemplate { get; set; } = null!;

	protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {

		return item switch {
			TextInputDataCollector => TextDataFieldTemplate,
			IntegerInputDataCollector => IntegerDataFieldTemplate,
			SelectionInputDataCollector => SelectionDataFieldTemplate,
			_ => throw new UnreachableException()
		};
	}

}



public class NullOrValueTemplateSelector : DataTemplateSelector {

	public required DataTemplate NullTemplate { get; init; } = null!;
	public required DataTemplate ValueTemplate { get; init; } = null!;

	protected override DataTemplate OnSelectTemplate(object? item, BindableObject container) {

		return item is null ? NullTemplate : ValueTemplate;
	}

}