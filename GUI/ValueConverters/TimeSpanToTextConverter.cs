using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GUI.ValueConverters;
internal class TimeSpanToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TimeSpan timespan)
        {
            return DependencyProperty.UnsetValue;
        }

        if (timespan == TimeSpan.Zero)
        {
            return "Searching...";
        }

        return "Search Time: " + timespan.ToString("s\\.fff") + "s";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
