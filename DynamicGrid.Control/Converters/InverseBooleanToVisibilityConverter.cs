using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DynamicGrid.Control.Converters;

public class InverseBooleanToVisibilityConverter : MarkupExtension, IValueConverter
{
    public bool IsInverse { get; set; }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var v = (bool)value;
        if (IsInverse)
        {
            return !v ? Visibility.Visible : Visibility.Collapsed;
        }

        return v ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}