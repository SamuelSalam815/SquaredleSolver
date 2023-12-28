using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfSquaredleSolver.ValueConverters;

public class IsCharacterTileInAnswerConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ;
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
