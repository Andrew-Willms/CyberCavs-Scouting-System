using System;
using System.Windows.Data;
using System.Globalization;
using WPFUtilities;
using CCSSDomain;

namespace GameMaker.Converters;

public class ErrorGreaterThanConverter : WPFUtilities.EnumGreaterThanConverter<CCSSDomain.ErrorSeverity> { }