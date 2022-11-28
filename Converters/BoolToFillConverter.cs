using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Battleships;

public class BoolToFillConverter : BaseValueConverter<BoolToFillConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Color c;
        if ((bool)value)
        {
            c = Color.FromRgb(0xff, 0, 0);
        }
        else
        {
            c = Color.FromRgb(0xcc, 0xcc, 0xcc);
        }

        return new SolidColorBrush(c);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
