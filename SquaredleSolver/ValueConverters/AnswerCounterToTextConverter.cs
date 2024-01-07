using SquaredleSolver.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SquaredleSolver.ValueConverters;

internal class AnswerCounterToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MainWindowViewModel.AnswerCounterStruct answerCounter)
        {
            return DependencyProperty.UnsetValue;
        }

        return $"Answers: {answerCounter.NumberDisplayed}/{answerCounter.NumberFound}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
