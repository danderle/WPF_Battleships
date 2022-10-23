using System;
using System.Globalization;
using System.Windows;

namespace Battleships;

public class InverseBoolToVisibilityConverter : BaseValueConverter<InverseBoolToVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return Visibility.Collapsed;
        }

        return Visibility.Visible;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
