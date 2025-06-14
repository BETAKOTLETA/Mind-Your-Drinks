using Mind_Your_Drink_Server.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Mind_Your_Drinks_App.ViewModels
{
    public class TodayViewModel : INotifyPropertyChanged
    {
        private Drink? selectedDrink;
        private bool isDrinkGridVisible;
        private bool isAddDrinkFormVisible;
        private string addButtonText = "Add Drink";

        public ObservableCollection<Drink> Drinks { get; } = new ObservableCollection<Drink>();

        public ICommand ToggleAddDrinkCommand { get; }
        public ICommand DrinkSelectedCommand { get; }
        public ICommand SaveDrinkCommand { get; }

        public bool IsDrinkGridVisible
        {
            get => isDrinkGridVisible;
            set => SetField(ref isDrinkGridVisible, value);
        }

        public bool IsAddDrinkFormVisible
        {
            get => isAddDrinkFormVisible;
            set => SetField(ref isAddDrinkFormVisible, value);
        }

        public Drink? SelectedDrink
        {
            get => selectedDrink;
            set => SetField(ref selectedDrink, value);
        }

        public string AddButtonText
        {
            get => addButtonText;
            set => SetField(ref addButtonText, value);
        }

        public TodayViewModel()
        {
            ToggleAddDrinkCommand = new RelayCommand(ToggleAddDrink);
            DrinkSelectedCommand = new RelayCommand<Drink>(OnDrinkSelected);
            SaveDrinkCommand = new RelayCommand(SaveDrink);

            AddDefaultDrinks();
        }

        private void ToggleAddDrink()
        {
            if (IsDrinkGridVisible || IsAddDrinkFormVisible)
            {
                // Cancel operation
                IsDrinkGridVisible = false;
                IsAddDrinkFormVisible = false;
                SelectedDrink = null;
                AddButtonText = "Add Drink";
            }
            else
            {
                // Show drink selection
                IsDrinkGridVisible = true;
                AddButtonText = "Cancel";
            }
        }

        private void OnDrinkSelected(Drink drink)
        {
            SelectedDrink = drink;
            IsDrinkGridVisible = false;
            IsAddDrinkFormVisible = true;
            AddButtonText = "Cancel";
        }

        private void SaveDrink()
        {
            // Save logic here
            IsAddDrinkFormVisible = false;
            SelectedDrink = null;
            AddButtonText = "Add Drink";
        }

        private void AddDefaultDrinks()
        {
            Drinks.Add(new Drink { Type = DrinkType.Beer, Name = "Beer" });
            Drinks.Add(new Drink { Type = DrinkType.Liquor, Name = "Liquor" });
            Drinks.Add(new Drink { Type = DrinkType.Beverage, Name = "Beverage" });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) =>
            _canExecute?.Invoke((T)parameter!) ?? true;

        public void Execute(object? parameter) => _execute((T)parameter!);

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}