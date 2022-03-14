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

public class ErrorToBrushConverter : IErrorConverter<ErrorSeverity, Brush> {

	public ReadOnlyDictionary<ErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<ErrorSeverity, Brush>() {
			{ ErrorSeverity.Note, Brushes.Gray },
			{ ErrorSeverity.Advisory, Brushes.Yellow },
			{ ErrorSeverity.Warning, Brushes.Orange },
			{ ErrorSeverity.Error, Brushes.Red }
		});

}
