using Microsoft.UI.Xaml.Data;
using System;
using System.ComponentModel;

namespace SteganographyInPicture.Converters;

internal class DoubleToByteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var byteValue = (byte)value; 
        return (double)byteValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var intValue = (int)Math.Round((double)value);

        return intValue > byte.MaxValue ? 
            byte.MaxValue :
            intValue < byte.MinValue ?
            byte.MinValue :
            (byte)intValue;
    }
}
