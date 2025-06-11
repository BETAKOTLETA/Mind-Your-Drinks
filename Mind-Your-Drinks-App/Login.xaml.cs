using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mind_Your_Drinks_App.Views
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();


        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {

            try
            {
                var url = "https://mind-your-drink-server20250611231310-h8aqf9a8cccgczfm.canadacentral-01.azurewebsites.net/User/Login";

                var loginData = new
                {
                    Name = UsernameEntry.Text,
                    Password = PasswordEntry.Text
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Application.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Failed", result, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}