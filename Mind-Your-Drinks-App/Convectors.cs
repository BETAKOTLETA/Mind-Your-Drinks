using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Mind_Your_Drinks_App.Converters
{
    public class StringIsNotNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}