using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace FrontMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddHttpClient("Api", client =>
        {
            var baseUrl = "http://192.160.0.6:5000";
            //var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
            //    ? "http://192.168.0.6:5000" // Android emulator loopback to host
            //    : "http://localhost:5000";
            client.BaseAddress = new Uri(baseUrl);
        });

        builder.Services.AddSingleton<Services.IApiClient, Services.ApiClient>();
        builder.Services.AddTransient<ViewModels.AddSpendViewModel>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}