using Mind_Your_Drink_Models.DTO_s;
using Mind_Your_Drink_Models.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Mind_Your_Drinks_App.Services;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class TodayViewModel : INotifyPropertyChanged
    {

        private const string ApiBaseUrl = "https://mind-your-drink-server20250611231310-h8aqf9a8cccgczfm.canadacentral-01.azurewebsites.net";
        private const string AddDrinkEndpoint = "UserDrink/CreateUserDrink";


        public ICommand ToggleAddDrinkCommand { get; }
        public ICommand DrinkSelectedCommand { get; }
        public ICommand SaveDrinkCommand { get; }


        private const float DailyEthanolTarget = 40f;
        private float _currentProgress;
        public float CurrentProgress
        {
            get => _currentProgress;
            set => SetField(ref _currentProgress, value);
        }

        private readonly ApiService _apiService = new(new HttpClient());

        private UserDrink? _selectedDrink;
        private bool _isDrinkGridVisible;
        private bool _isAddDrinkFormVisible;
        private string _addButtonText = "Add Drink";


        public ObservableCollection<Drink> Drinks { get; } = new();
        public User? CurrentUser { get; set; } // Should be set when user logs in

        public bool IsDrinkGridVisible
        {
            get => _isDrinkGridVisible;
            set => SetField(ref _isDrinkGridVisible, value);
        }

        public bool IsAddDrinkFormVisible
        {
            get => _isAddDrinkFormVisible;
            set => SetField(ref _isAddDrinkFormVisible, value);
        }

        public UserDrink? SelectedDrink
        {
            get => _selectedDrink;
            set => SetField(ref _selectedDrink, value);
        }

        public string AddButtonText
        {
            get => _addButtonText;
            set => SetField(ref _addButtonText, value);
        }

        public TodayViewModel()
        {

            ToggleAddDrinkCommand = new RelayCommand(ToggleAddDrinkForm);
            DrinkSelectedCommand = new RelayCommand<Drink>(SelectDrink);
            SaveDrinkCommand = new RelayCommand(SaveSelectedDrink);

            LoadDefaultDrinks();
        }

        private void LoadDefaultDrinks()
        {
            Drinks.Add(new Drink { Type = DrinkType.Beer, Name = "Beer", Icon = "beer" });
            Drinks.Add(new Drink { Type = DrinkType.Wine, Name = "Wine", Icon = "wine" });
            Drinks.Add(new Drink { Type = DrinkType.Cider, Name = "Cider", Icon = "cider"});
            Drinks.Add(new Drink { Type = DrinkType.Vodka, Name = "Vodka", Icon = "vodka"});
            Drinks.Add(new Drink { Type = DrinkType.Tequila, Name = "Tequila", Icon = "tequila"});
            Drinks.Add(new Drink { Type = DrinkType.Whiskey, Name = "Whiskey", Icon = "whiskey"});
            Drinks.Add(new Drink { Type = DrinkType.Rum, Name = "Rum", Icon = "rum"});
            Drinks.Add(new Drink { Type = DrinkType.Brandy, Name = "Brandy", Icon = "brandy" });
            Drinks.Add(new Drink { Type = DrinkType.Gin, Name = "Gin", Icon = "gin"});
            Drinks.Add(new Drink { Type = DrinkType.Liqueur, Name = "Liqueur", Icon = "liqueur"});
            Drinks.Add(new Drink { Type = DrinkType.MixedDrink, Name = "Mixed Drink", Icon = "mixeddrink"});
            Drinks.Add(new Drink { Type = DrinkType.Other, Name = "Other"});
        }

        private void ToggleAddDrinkForm()
        {
            if (IsDrinkGridVisible || IsAddDrinkFormVisible)
            {
                ResetDrinkSelection();
            }
            else
            {
                ShowDrinkSelection();
            }
        }

        private void ResetDrinkSelection()
        {
            IsDrinkGridVisible = false;
            IsAddDrinkFormVisible = false;
            SelectedDrink = null;
            AddButtonText = "Add Drink";
        }

        private void ShowDrinkSelection()
        {
            IsDrinkGridVisible = true;
            AddButtonText = "Cancel";
        }

        private void SelectDrink(Drink drink)
        {
            if (drink == null) return;

            SelectedDrink = new UserDrink
            {
                Name = drink.Name,
                Type = drink.Type,
                Time = TimeZoneInfo.ConvertTimeFromUtc(
                   DateTime.UtcNow,
                   TimeZoneInfo.FindSystemTimeZoneById(
                       OperatingSystem.IsAndroid() ? "Europe/Warsaw"   // IANA ID (Android/Linux)
                                                   : "Central European Standard Time")), // Windows/iOS ID
                Icon = drink.Icon

            };

            IsDrinkGridVisible = false;
            IsAddDrinkFormVisible = true;
            AddButtonText = "Cancel";
        }

        private async void SaveSelectedDrink()
        {
            if (SelectedDrink == null || GlobalState.CurrentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No drink or user selected", "OK");
                return;
            }

            try
            {
                var success = await SaveDrinkToServer(GlobalState.CurrentUser, SelectedDrink);
                if (success)
                {
                    ResetDrinkSelection();
                    await LoadProgressAsync(); 
                }
            }
            catch (Exception ex)
            {
                await HandleError(ex);
            }
        }


        private async Task<bool> SaveDrinkToServer(User user, UserDrink userDrink)
        {
            using var client = CreateHttpClient();
            var request = CreateUserDrinkRequest(user, userDrink);

            var response = await client.PostAsync(
                $"{ApiBaseUrl}/{AddDrinkEndpoint}",
                SerializeRequest(request)
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Server error: {error}");
            }

            return true;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            return client;
        }

        private UserDrinkRequest CreateUserDrinkRequest(User user, UserDrink userDrink)
        {
            return new UserDrinkRequest
            {
                Name = user.Name,
                Hash = user.HashPassword,
                UserDrink = userDrink
            };
        }

        private StringContent SerializeRequest(UserDrinkRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task HandleError(Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                $"An error occurred: {ex.Message}",
                "OK"
            );
        }
        public async Task LoadProgressAsync()
        {
            if (GlobalState.CurrentUser == null)
                return;

            var drinks = await _apiService.GetDrinksByDay(
                GlobalState.CurrentUser.Name,
                GlobalState.CurrentUser.HashPassword,
                DateTime.Today);

            float totalEthanol = 0;

            //var testDrinks = new List<UserDrink> {
            //new UserDrink { Name = "beer", VolumeInMl = 100, Abv = 3.0f }
            //    };

            foreach (var drink in drinks ?? Enumerable.Empty<UserDrink>())
            {
                float ethanol = drink.VolumeInMl * (drink.Abv / 100f);
                totalEthanol += ethanol;

                //await Application.Current.MainPage.DisplayAlert("Drink", $"{drink.VolumeInMl}ml @ {drink.Abv}% = {ethanol}g ethanol", "OK");
            } 

            
            float progress = totalEthanol / DailyEthanolTarget;

            CurrentProgress = Math.Min(progress, 20f);
        }



        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }

    // Command implementations
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter) => _execute();
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke((T)parameter!) ?? true;
        public void Execute(object? parameter) => _execute((T)parameter!);
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}