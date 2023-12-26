using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfSquaredleSolver.ValueConverters;

/// <summary>
///     Determines the text for the button that toggles the puzzle solver on or off
/// </summary>
internal class IsSolverRunningTextConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            return DependencyProperty.UnsetValue;
        }

        if (boolValue)
        {
            return "Stop Solving Puzzle";
        }

        return "Start Solving Puzzle";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}