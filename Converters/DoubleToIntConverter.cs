using Microsoft.UI.Xaml.Data;
using System;

namespace SteganographyInPicture.Converters;

internal class DoubleToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return double.Parse(value?.ToString() ?? "0");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (Double.IsNaN((double)value)) return 0;
        return value == default ? 0 : (int)Math.Round((double)value, MidpointRounding.ToPositiveInfinity);
    }
}
