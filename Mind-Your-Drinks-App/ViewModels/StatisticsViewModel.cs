using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public StatisticsViewModel()
        {

        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        private string _totalEthanolText = "Total Ethanol: 0 ml";
        public string TotalEthanolText
        {
            get => _totalEthanolText;
            set => SetField(ref _totalEthanolText, value);
        }

        private string _totalCaloriesText = "Total Calories: 0";
        public string TotalCaloriesText
        {
            get => _totalCaloriesText;
            set => SetField(ref _totalCaloriesText, value);
        }

        private string _totalPriceText = "Total Price: $0.00";
        public string TotalPriceText
        {
            get => _totalPriceText;
            set => SetField(ref _totalPriceText, value);
        }

        private string _periodLabel = "Showing: Today";
        public string PeriodLabel
        {
            get => _periodLabel;
            set => SetField(ref _periodLabel, value);
        }

        public ICommand TodayCommand { get; }
        public ICommand WeekCommand { get; }
        public ICommand MonthCommand { get; }
        public ICommand YearCommand { get; }

        public StatisticsViewModel(ApiService apiService)
        {
            _apiService = apiService;

            TodayCommand = new Command(async () => await LoadDrinksForPeriod("today"));
            WeekCommand = new Command(async () => await LoadDrinksForPeriod("week"));
            MonthCommand = new Command(async () => await LoadDrinksForPeriod("month"));
            YearCommand = new Command(async () => await LoadDrinksForPeriod("year"));

            _ = LoadDrinksForPeriod("today"); // Load today's stats by default
        }


        private async Task LoadDrinksForPeriod(string period)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                PeriodLabel = $"Showing: {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(period)}";

                IEnumerable<UserDrink> drinks = period switch
                {
                    "today" => await _apiService.GetTodayDrinksAsync(GlobalState.CurrentUser.Name),
                    "week" => await _apiService.GetCurrentWeekDrinksAsync(GlobalState.CurrentUser.Name),
                    "month" => await _apiService.GetCurrentMonthDrinksAsync(GlobalState.CurrentUser.Name),
                    "year" => await _apiService.GetCurrentYearDrinksAsync(GlobalState.CurrentUser.Name),
                    _ => Enumerable.Empty<UserDrink>()
                };

                CalculateAndDisplayTotals(drinks);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateAndDisplayTotals(IEnumerable<UserDrink> drinks)
        {
            double totalEthanol = 0;
            double totalCalories = 0;
            decimal totalPrice = 0;

            foreach (var drink in drinks)
            {
                totalEthanol += drink.VolumeInMl * (drink.Abv / 100.0);
                totalCalories += drink.Calories;
                totalPrice += drink.Price;
            }

            TotalEthanolText = $"Total Ethanol: {totalEthanol:F2} ml";
            TotalCaloriesText = $"Total Calories: {totalCalories}";
            TotalPriceText = $"Total Price: {totalPrice:C}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}