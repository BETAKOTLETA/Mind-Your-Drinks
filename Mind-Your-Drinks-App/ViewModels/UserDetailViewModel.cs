using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class UserDetailViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BanButtonText));
            }
        }

        public string BanButtonText => User?.StateName == "Banned" ? "Unban User" : "Ban User";

        public ICommand ToggleBanCommand { get; }
        public ICommand GoBackCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserDetailViewModel(ApiService apiService, User user)
        {
            _apiService = apiService;
            User = user;

            ToggleBanCommand = new Command(async () => await ToggleBanStatus());
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task ToggleBanStatus()
        {
            string adminName = GlobalState.CurrentUser.Name;
            string adminPassword = GlobalState.CurrentUser.HashPassword;

            if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Admin credentials not found", "OK");
                return;
            }

            var action = User.StateName == "Banned" ? "unban" : "ban";
            var confirm = await Shell.Current.DisplayAlert(
                "Confirm Action",
                $"Are you sure you want to {action} {User.Name}?",
                "Yes", "No"
            );

            if (confirm)
            {
                bool success;
                if (User.StateName != "Banned")
                {
                    // Ban user
                    success = await _apiService.BanUserAsync(User.Name, adminPassword);
                }
                else
                {
                    // Unban user
                    success = await _apiService.UnBanUserAsync(User.Name, adminPassword);
                }

                if (success)
                {
                    // Update local state
                    User.StateName = User.StateName == "Banned" ? "Active" : "Banned";
                    OnPropertyChanged(nameof(User));
                    OnPropertyChanged(nameof(BanButtonText));

                    await Shell.Current.DisplayAlert(
                        "Success",
                        $"{User.Name} has been {action}ned",
                        "OK"
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                        $"Failed to {action} user. Please try again later.",
                        "OK"
                    );
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}