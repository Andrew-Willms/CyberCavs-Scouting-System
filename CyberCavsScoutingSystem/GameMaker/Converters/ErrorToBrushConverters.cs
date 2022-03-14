using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCSSDomain;
using WPFUtilities;

namespace GameMaker.Converters;

public class ErrorToToolTipTextBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush>() {
			{ ErrorSeverity.Note, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Yellow },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToNormalBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneNormalBrush = new((Color) ColorConverter.ConvertFromString("#FFABADD3"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush>() {
			{ ErrorSeverity.None, NoneNormalBrush },
			{ ErrorSeverity.Note, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Yellow },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToMouseOverBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneMouseOverBrush = new((Color)ColorConverter.ConvertFromString("#FF7EB4EA"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush>() {
			{ ErrorSeverity.None, NoneMouseOverBrush },
			{ ErrorSeverity.Note, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Yellow },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

public class ErrorToFocusedBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	private static readonly SolidColorBrush NoneFocusedBrush = new((Color)ColorConverter.ConvertFromString("#FF569DE5"));

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush>() {
			{ ErrorSeverity.None, NoneFocusedBrush },
			{ ErrorSeverity.Note, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Yellow },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});
}

