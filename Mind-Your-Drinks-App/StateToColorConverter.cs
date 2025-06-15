using System.Globalization;

namespace Mind_Your_Drinks_App.Converters
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string) == "Banned" ? Color.FromArgb("#BF616A") : Color.FromArgb("#A3BE8C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}