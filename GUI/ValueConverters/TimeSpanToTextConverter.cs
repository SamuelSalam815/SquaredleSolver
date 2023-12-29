using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfSquaredleSolver.ValueConverters;
internal class TimeSpanToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TimeSpan timespan)
        {
            return DependencyProperty.UnsetValue;
        }

        return "Search Time: " + timespan.ToString("s\\.fff") + "s";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
