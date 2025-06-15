using Microsoft.Extensions.Logging;

using Mind_Your_Drink_Server.Models;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Core.Hosting;

using Mind_Your_Drink_Models.Models;
using Mind_Your_Drinks_App.ViewModels;
using Mind_Your_Drinks_App.Services;

namespace Mind_Your_Drinks_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HttpClient>();

            // Register API Service
            builder.Services.AddSingleton<ApiService>();

            // Register ViewModels
            builder.Services.AddTransient<AdminViewModel>();

            //// Register Views
            builder.Services.AddTransient<Admin>();


            return builder.Build();
        }
    }
}
