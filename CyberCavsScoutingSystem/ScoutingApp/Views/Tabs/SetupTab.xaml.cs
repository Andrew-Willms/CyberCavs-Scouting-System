using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Tabs; 



public partial class SetupTab : ContentPage {

	public ObservableCollection<DataField> Inputs { get; } = new(new() {
		new IntegerDataField(new() { Name = "High Cones", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
		new IntegerDataField(new() { Name = "Mid Cones", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
		new IntegerDataField(new() { Name = "Low Cones", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
		new IntegerDataField(new() { Name = "High Cubes", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
		new IntegerDataField(new() { Name = "Mid Cubes", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
		new IntegerDataField(new() { Name = "Low Cubes", InitialValue = 0, MinValue = 0, MaxValue = 10 }),
	});

	public SetupTab() {

		BindingContext = this;
		InitializeComponent();
	}

}