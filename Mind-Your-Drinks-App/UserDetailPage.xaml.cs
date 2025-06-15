using Mind_Your_Drinks_App.Services; // Add this namespace reference
using Mind_Your_Drinks_App.ViewModels;
using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drinks_App;

public partial class UserDetailPage : ContentPage
{
	public UserDetailPage(ApiService apiService, User user)
	{
		InitializeComponent();
        BindingContext = new UserDetailViewModel(apiService, user);
    }
}