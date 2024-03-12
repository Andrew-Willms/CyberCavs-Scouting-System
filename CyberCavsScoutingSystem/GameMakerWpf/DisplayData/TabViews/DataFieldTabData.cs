using System.Windows.Media;

namespace GameMakerWpf.DisplayData.TabViews;



public class DataFieldTabData {

	public static string TabTitle => "Data Fields";

	public static string DataFieldsText => "Data Fields";

	public static string AddText => "Add";

	public static string RemoveText => "Remove";

	public static string MoveUpText => "Move Up";

	public static string MoveDownText => "MoveDown";

	public static SolidColorBrush PropertyPanelBorderBrush => new((Color)ColorConverter.ConvertFromString("#FFABADD3"));

}

public class TextDataFieldData {

	public static string InitialValueText => "Initial Value";

	public static string MustNotBeEmptyText => "Must Not Be Empty";

	public static string MustNotBeInitialValueText => "Must Not Be Initial Value";

}

public class IntegerDataFieldData {

	public static string InitialValueText => "Initial Value";

	public static string MinValueText => "Min Value";

	public static string MaxValueText => "Max Value";

	public static string MinValueButtonText => "Min";

	public static string MaxValueButtonText => "Max";

}

public class SelectionDataFieldData {

	public static string OptionsText => "Selection";

	public static string AddText => "Add";

	public static string RemoveText => "Remove";

	public static string MoveUpText => "Move Up";

	public static string MoveDownText => "MoveDown";

}