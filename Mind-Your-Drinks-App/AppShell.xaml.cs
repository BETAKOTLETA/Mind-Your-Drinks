namespace Mind_Your_Drinks_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Set initial admin state
            AdminTab.IsVisible = GlobalState.isAdmin;   

            

        }

        public void OnAdminStatusChanged(object sender, EventArgs e)
        {
            
            MainThread.BeginInvokeOnMainThread(() => {
                AdminTab.IsVisible = GlobalState.isAdmin;
            });
        }

        // Cleanup when Shell is disposed
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
    }
}