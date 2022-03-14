using System;
using System.Windows.Data;
using System.Globalization;
using WPFUtilities;
using CCSSDomain;

namespace GameMaker.Converters;

public class ErrorGreaterThanConverter : WPFUtilities.EnumGreaterThanConverter<CCSSDomain.ErrorSeverity> { }

/* Here is the XAML code for how this style was used

    <!--<Converters:ErrorGreaterThanConverter x:Key="ErrorGreaterThanConverter"/>-->

    <!--<Style.Triggers>
        <DataTrigger Binding="{Binding Path=ErrorLevel,
                     Converter={StaticResource ErrorGreaterThanConverter},
                     ConverterParameter={x:Static Domain:ErrorSeverity.None}}"
                     Value="True">
            <Setter Property="BorderBrush" Value="{Binding Path=ErrorLevel, Converter={StaticResource ErrorToNormalBrushConverter}, Mode=OneWay}"/>
        </DataTrigger>
    </Style.Triggers>-->

*/