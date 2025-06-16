using Mind_Your_Drinks_App.Views;

namespace Mind_Your_Drinks_App;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        GlobalState.Reset();
        await Shell.Current.GoToAsync("//login");
    }
}