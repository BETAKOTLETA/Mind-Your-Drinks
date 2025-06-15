using Mind_Your_Drink_Models;
using Mind_Your_Drink_Models.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Mind_Your_Drinks_App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://mind-your-drink-server20250611231310-h8aqf9a8cccgczfm.canadacentral-01.azurewebsites.net/";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetAllUsersAsync(string adminName, string adminPassword)
        {
            try
            {
                var request = new UserCreateRequest
                {
                    Name = adminName,
                    Password = adminPassword
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}Admin/GetAllUserInfo",
                    request
                );

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<User>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, string newStatus, string adminName, string adminPassword)
        {
            try
            {
                var request = new
                {
                    UserId = userId,
                    NewStatus = newStatus,
                    AdminCredentials = new UserCreateRequest
                    {
                        Name = adminName,
                        Password = adminPassword
                    }
                };

                var response = await _httpClient.PutAsJsonAsync(
                    $"{BaseUrl}Admin/UpdateUserStatus",
                    request
                );

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> BanUserAsync(string toBanName, string adminPassword)
        {
            try
            {
                var request = new
                {
                    ToBanName = toBanName,
                    AdminPassword = adminPassword
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}Admin/Ban",
                    request
                );

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UnBanUserAsync(string toBanName, string adminPassword)
        {
            try
            {
                var request = new
                {
                    ToBanName = toBanName,
                    AdminPassword = adminPassword
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}Admin/UnBan",
                    request
                );

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

    }

    public class UserCreateRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}