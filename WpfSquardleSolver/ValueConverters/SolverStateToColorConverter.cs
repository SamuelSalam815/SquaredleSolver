using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ValueConverters;

/// <summary>
///     Determines the color for the button that toggles the puzzle solver on or off 
/// </summary>
internal class SolverStateToColorConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ISolverState state)
        {
            return DependencyProperty.UnsetValue;
        }

        return state switch
        {
            SolverRunning => new BrushConverter().ConvertFrom("#e74c3c") as SolidColorBrush,// Red
            SolverStopped => new BrushConverter().ConvertFrom("#2ecc71") as SolidColorBrush,// Green
            SolverCompleted => new BrushConverter().ConvertFrom("#f1c40f") as SolidColorBrush,// Yellow
            _ => DependencyProperty.UnsetValue,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
