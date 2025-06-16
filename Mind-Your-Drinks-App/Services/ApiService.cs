using Mind_Your_Drink_Models;
using Mind_Your_Drink_Models.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task<IEnumerable<UserDrink>> GetDrinksByDay(string name, string password, DateTime date)
        {
            try
            {
                var request = new
                {
                    Name = name,
                    Date = date.ToString("yyyy-MM-dd")
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}UserDrink/GetDrinksByDay",
                    request
                );

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<UserDrink>();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DateTimeConverter() }
                };

                var drinks = await response.Content.ReadFromJsonAsync<IEnumerable<UserDrink>>(options);
                return drinks ?? Enumerable.Empty<UserDrink>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.ToString(), "OK");
                return Enumerable.Empty<UserDrink>();
            }
        }

        public async Task<IEnumerable<UserDrink>> GetTodayDrinksAsync(string name)
        {
            //await Application.Current.MainPage.DisplayAlert("I am heer","gg", "OK");
            return await GetDrinksByPeriod(name, "today");
        }

        public async Task<IEnumerable<UserDrink>> GetCurrentWeekDrinksAsync(string name)
        {
            return await GetDrinksByPeriod(name, "week");
        }

        public async Task<IEnumerable<UserDrink>> GetCurrentMonthDrinksAsync(string name)
        {
            return await GetDrinksByPeriod(name, "month");
        }

        public async Task<IEnumerable<UserDrink>> GetCurrentYearDrinksAsync(string name)
        {
            return await GetDrinksByPeriod(name, "year");
        }

        private async Task<IEnumerable<UserDrink>> GetDrinksByPeriod(string name, string period)
        {
            try
            {
                var request = new
                {
                    Name = name,
                    Period = period
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}UserDrink/GetDrinksByPeriod",
                    request
                );

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<UserDrink>();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DateTimeConverter() }
                };
                var drinks = await response.Content.ReadFromJsonAsync<IEnumerable<UserDrink>>(options)
                       ?? Enumerable.Empty<UserDrink>();

                foreach (var item in drinks)
                {
                    ///*await Application.Current.MainPage.DisplayAlert(item.Name, item*/.Time.ToString(), "Ok");
                }

                return drinks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching {period} drinks: {ex.Message}");
                return Enumerable.Empty<UserDrink>();
            }
        }

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("o"));
            }
        }
    }


    public class UserCreateRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}