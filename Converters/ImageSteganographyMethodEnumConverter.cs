using Microsoft.UI.Xaml.Data;
using SteganographyInPicture.Enums;
using System;

namespace SteganographyInPicture.Converters;

public class ImageSteganographyMethodEnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null || parameter == null)
            return false;

        // Возвращаем true, если значение из перечисления совпадает с параметром радиокнопки
        return value.ToString().Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if ((bool)value)
        {
            return Enum.Parse(typeof(ImageSteganographyMethodEnum), parameter.ToString());
        }

        return 0;
    }
}