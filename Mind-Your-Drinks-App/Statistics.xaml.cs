using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.Services;
using Mind_Your_Drinks_App.ViewModels;
using System.Globalization;

namespace Mind_Your_Drinks_App;

public partial class Statistics : ContentPage
{

    public Statistics()
    {
        InitializeComponent();

        var httpClient = new HttpClient();
        var apiService = new ApiService(httpClient);

        BindingContext = new StatisticsViewModel(apiService);
        Resources.Add("StringIsNotNullOrEmptyConverter", new StringIsNotNullOrEmptyConverter());

    }

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