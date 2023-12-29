using SquaredleSolver.SolverStates;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI.ValueConverters;

/// <summary>
///     Determines the text for the button that toggles the puzzle solver on or off
/// </summary>
internal class SolverStateToTextConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ISolverState state)
        {
            return DependencyProperty.UnsetValue;
        }

        return state switch
        {
            SolverRunning => "Stop Solving Puzzle",
            SolverStopped => "Start Solving Puzzle",
            SolverCompleted => "Puzzle Explored",
            _ => DependencyProperty.UnsetValue,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}