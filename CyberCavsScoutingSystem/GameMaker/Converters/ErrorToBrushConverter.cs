using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities;



namespace GameMaker.Converters;

//public class ErrorToBrushConverter : IErrorConverter {

//	public ReadOnlyDictionary<StringInputValidationErrorSeverity, object> ConversionDictionary { get; } =
//		new(new Dictionary<StringInputValidationErrorSeverity, object>() {
//			{ StringInputValidationErrorSeverity.Note, Brushes.Gray },
//			{ StringInputValidationErrorSeverity.Advisory, Brushes.Yellow },
//			{ StringInputValidationErrorSeverity.Warning, Brushes.Orange },
//			{ StringInputValidationErrorSeverity.Error, Brushes.Red }
//		});
//}

public class ErrorToBrushConverter : IErrorConverter<Brush> {

	public ReadOnlyDictionary<StringInputValidationErrorSeverity, Brush> ConversionDictionary { get; } =
		new(new Dictionary<StringInputValidationErrorSeverity, Brush>() {
			{ StringInputValidationErrorSeverity.Note, Brushes.Gray },
			{ StringInputValidationErrorSeverity.Advisory, Brushes.Yellow },
			{ StringInputValidationErrorSeverity.Warning, Brushes.Orange },
			{ StringInputValidationErrorSeverity.Error, Brushes.Red }
		});

}
