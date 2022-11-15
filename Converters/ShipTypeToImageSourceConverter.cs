using System;
using System.Diagnostics;
using System.Globalization;

namespace Battleships;

public class ShipTypeToImageSourceConverter : BaseValueConverter<ShipTypeToImageSourceConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string path = string.Empty;
        switch ((ShipTypes)value)
        {
            case ShipTypes.Submarine:
                path = "Resources/Images/Submarine.jpg";
                break;
            default:
                Debugger.Break();
                break;
        }

        return path;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
