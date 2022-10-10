using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using CCSSDomain;
using UtilitiesLibrary.WPF;

namespace GameMakerWpf.Converters;



public class ErrorToToolTipTextBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush> {
			{ ErrorSeverity.None, Brushes.Blue }, // this shouldn't be used but is here to prevent null exceptions and help with debugging
			{ ErrorSeverity.Info, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Gold },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToNormalBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneNormalBrush = new((Color)ColorConverter.ConvertFromString("#FFABADD3"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush> {
			{ ErrorSeverity.None, NoneNormalBrush },
			{ ErrorSeverity.Info, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Gold },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToMouseOverBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneMouseOverBrush = new((Color)ColorConverter.ConvertFromString("#FF7EB4EA"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush> {
			{ ErrorSeverity.None, NoneMouseOverBrush },
			{ ErrorSeverity.Info, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Gold },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToFocusedBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneFocusedBrush = new((Color)ColorConverter.ConvertFromString("#FF569DE5"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush> {
			{ ErrorSeverity.None, NoneFocusedBrush },
			{ ErrorSeverity.Info, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Gold },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}