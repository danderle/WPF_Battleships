using System;
using System.Diagnostics;
using System.Globalization;

namespace Battleships;

/// <summary>
/// Converts a <see cref="ApplicationPages"/> enum to an actual page
/// </summary>
public class ApplicationPagesConverter : BaseValueConverter<ApplicationPagesConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var page = (ApplicationPages)value;

        switch (page)
        {
            case ApplicationPages.MainMenuPage:
                return new MainMenuPage();

            case ApplicationPages.ShipPlacementPage:
                return new ShipPlacementPage();

            default:
                Debugger.Break();
                return null;
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
