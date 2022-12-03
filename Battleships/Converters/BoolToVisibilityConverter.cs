using System;
using System.Globalization;
using System.Windows;

namespace Battleships;

public class BoolToVisibilityConverter : BaseValueConverter<BoolToVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
