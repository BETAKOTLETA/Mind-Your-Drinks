using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.ViewModels;

namespace Mind_Your_Drinks_App;

public partial class Today : ContentPage
{
    private CircularProgressDrawable _progressDrawable;

    public Today()
    {
        InitializeComponent();
        var vm = new TodayViewModel();
        BindingContext = vm;

        _progressDrawable = new CircularProgressDrawable();
        ProgressView.Drawable = _progressDrawable;

        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.CurrentProgress))
                UpdateProgress(vm.CurrentProgress);
        };

        Loaded += async (_, __) =>
        {
            await vm.LoadProgressAsync();
        };


    }

    private void UpdateProgress(float progress)
    {
        _progressDrawable.Progress = progress;
        ProgressView.Invalidate();
    }

    private void OnDrinkSelected(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is UserDrink drink)
        {
            var vm = (TodayViewModel)BindingContext;
            vm.SelectedDrink = drink;
            vm.IsDrinkGridVisible = false;
            vm.IsAddDrinkFormVisible = true;
            vm.AddButtonText = "Cancel";
        }
    }
}