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
            case ShipTypes.Destroyer:
                path = "Resources/Images/Destroyer.jpg";
                break;
            case ShipTypes.Cruiser:
                path = "Resources/Images/Cruiser.jpg";
                break;
            case ShipTypes.Battleship:
                path = "Resources/Images/Battleship.jpg";
                break;
            case ShipTypes.Carrier:
                path = "Resources/Images/Carrier.jpg";
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
