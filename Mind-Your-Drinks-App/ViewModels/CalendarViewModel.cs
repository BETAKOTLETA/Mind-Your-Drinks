using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private DateTime? _selectedDate;
        private readonly ApiService _apiService;
        private ObservableCollection<UserDrink> _drinksForSelectedDate = new();

        public CalendarViewModel(ApiService apiService)
        {
            _apiService = apiService;
            SelectedDate = DateTime.Today;
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    if (value.HasValue)
                        LoadDrinksForDate(value.Value);
                }
            }
        }

        public ObservableCollection<UserDrink> DrinksForSelectedDate
        {
            get => _drinksForSelectedDate;
            set
            {
                _drinksForSelectedDate = value;
                OnPropertyChanged();
/*                OnPropertyChanged(nameof(TotalCalories));*/ // Update calculated property
            }
        }

        //public int TotalCalories => DrinksForSelectedDate.Sum(d => d.Calories);

        private async void LoadDrinksForDate(DateTime date)
        {
            try
            {
                var drinks = await _apiService.GetDrinksByDay(
                    GlobalState.CurrentUser.Name,
                    GlobalState.CurrentUser.HashPassword,
                    date
                );

                DrinksForSelectedDate = new ObservableCollection<UserDrink>(drinks);

                if (DrinksForSelectedDate.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Drink Info",
                        $"{DrinksForSelectedDate[0].Name} - ABV: {DrinksForSelectedDate[0].Abv}%",
                        "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Info",
                        "No drinks found for this date",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Failed to load drinks: {ex.Message}",
                    "OK");
                DrinksForSelectedDate.Clear();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}