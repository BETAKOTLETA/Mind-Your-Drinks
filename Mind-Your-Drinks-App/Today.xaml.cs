using Mind_Your_Drink_Server.Models;
using Mind_Your_Drinks_App.ViewModels;

namespace Mind_Your_Drinks_App;

public partial class Today : ContentPage
{
    public Today()
    {
        InitializeComponent();
        BindingContext = new TodayViewModel();
    }
    private void OnDrinkSelected(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Drink drink)
        {
            var vm = (TodayViewModel)BindingContext;
            vm.SelectedDrink = drink;
            vm.IsDrinkGridVisible = false;
            vm.IsAddDrinkFormVisible = true;
            vm.AddButtonText = "Cancel";
        }
    }
}