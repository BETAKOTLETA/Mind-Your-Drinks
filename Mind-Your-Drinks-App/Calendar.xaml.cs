using Mind_Your_Drinks_App.Services;
using Mind_Your_Drinks_App.ViewModels;

namespace Mind_Your_Drinks_App;

public partial class Calendar : ContentPage
    {
        public Calendar()
        {
            InitializeComponent();
            calendar.DisplayDate = DateTime.Today; // set initial month
            var httpClient = new HttpClient();
            var apiService = new ApiService(httpClient);
            BindingContext = new CalendarViewModel(apiService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is CalendarViewModel vm)
            await vm.RefreshAsync();   
        }
}

