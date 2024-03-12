using GameMakerWpf.Domain;
using WPFUtilities;

namespace GameMakerWpf.WpfConverters;



public class ErrorGreaterThanConverter : EnumGreaterThanConverter<ErrorSeverity> { }

/* Here is the XAML code for how this style was used

    <!--<Converters:ErrorGreaterThanConverter x:Key="ErrorGreaterThanConverter"/>-->

    <!--<Style.Triggers>
        <DataTrigger Binding="{Binding Path=ValidationErrorLevel,
                     Converter={StaticResource ErrorGreaterThanConverter},
                     ConverterParameter={x:Static Domain:ErrorSeverity.None}}"
                     Value="True">
            <Setter Property="BorderBrush" Value="{Binding Path=ValidationErrorLevel, Converter={StaticResource ErrorToNormalBrushConverter}, Mode=OneWay}"/>
        </DataTrigger>
    </Style.Triggers>-->

*/