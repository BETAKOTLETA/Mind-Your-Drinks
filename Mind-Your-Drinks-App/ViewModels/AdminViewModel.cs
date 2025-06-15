using Mind_Your_Drink_Models;
using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private ObservableCollection<User> _allUsers = new ObservableCollection<User>();
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        private string _searchText = string.Empty;
        private bool _isBusy = false;
        private bool _isRefreshing = false;
        private User _selectedUser;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();

                if (_selectedUser != null)
                {
                    NavigateToUserDetail(_selectedUser);
                    // Reset selection
                    _selectedUser = null;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }

        public ICommand LoadUsersCommand { get; }
        public ICommand RefreshCommand { get; }

        public AdminViewModel(ApiService apiService)
        {
            _apiService = apiService;

            LoadUsersCommand = new Command(async () => await LoadUsers());
            RefreshCommand = new Command(async () => await RefreshData());

            Task.Run(LoadUsers);
        }

        public AdminViewModel() : this(new ApiService(new HttpClient()))
        {
        }

        private async Task LoadUsers()
        {
            try
            {
                IsBusy = true;
                _allUsers.Clear();

                string adminName = GlobalState.CurrentUser.Name;
                string adminPassword = GlobalState.CurrentUser.HashPassword;

                if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminPassword))
                {
                    await Shell.Current.DisplayAlert("Error", "Admin credentials not found", "OK");
                    return;
                }

                var users = await _apiService.GetAllUsersAsync(adminName, adminPassword);

                if (users == null || users.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Info", "No users found", "OK");
                    return;
                }

                foreach (var user in users)
                {
                    _allUsers.Add(user);
                }

                FilterUsers();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private async Task RefreshData()
        {
            IsRefreshing = true;
            await LoadUsers();
        }

        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Users = new ObservableCollection<User>(_allUsers);
            }
            else
            {
                Users = new ObservableCollection<User>(
                    _allUsers.Where(u =>
                        (u.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (u.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
                );
            }
        }

        private async void NavigateToUserDetail(User user)
        {
            var detailPage = new UserDetailPage(_apiService, user);
            await Shell.Current.Navigation.PushAsync(detailPage);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}