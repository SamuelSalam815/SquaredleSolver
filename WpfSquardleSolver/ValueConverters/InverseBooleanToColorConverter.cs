using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfSquardleSolver.ValueConverters;

/// <summary>
///     Converts true values to red and false values to green 
/// </summary>
internal class InverseBooleanToColorConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            return DependencyProperty.UnsetValue;
        }

        if (!targetType.IsAssignableTo(typeof(Brush)))
        {
            return DependencyProperty.UnsetValue;
        }

        if (boolValue)
        {
            return new BrushConverter().ConvertFrom("#e74c3c") as SolidColorBrush; // Red
        }

        return new BrushConverter().ConvertFrom("#2ecc71") as SolidColorBrush; // Green
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
