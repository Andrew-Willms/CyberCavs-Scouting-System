using System.Collections.ObjectModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages; 



public partial class TeleTab : ContentPage {

	public static string Route => "Tele";

	public ObservableCollection<DataField> Inputs { get; } = new(new() {
		new IntegerDataField(new() { Name = "High Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "High Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Mid Cones", InitialValue = 0, MinValue = 0, MaxValue = 6 }),
		new IntegerDataField(new() { Name = "Mid Cubes", InitialValue = 0, MinValue = 0, MaxValue = 3 }),
		new IntegerDataField(new() { Name = "Low Cones", InitialValue = 0, MinValue = 0, MaxValue = 9 }),
		new IntegerDataField(new() { Name = "Low Cubes", InitialValue = 0, MinValue = 0, MaxValue = 9 })
	});

	public TeleTab() {

		BindingContext = this;
		InitializeComponent();
	}

}