using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SquaredleSolver.ValueConverters;

internal class NumberOfAnswersFoundToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int answerCount)
        {
            return DependencyProperty.UnsetValue;
        }

        return $"Answers found: {answerCount}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
