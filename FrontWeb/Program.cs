using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// If I want run in Dev environment, set for true
bool IsDevelopmentEnvironment = builder.HostEnvironment.IsDevelopment();

//string API_BASE_URL_DEV = "http://localhost:44383/";
//string API_BASE_URL_PROD = "http://localhost:5000/";

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<HttpClient>(sp =>
{
    //var configuration = sp.GetRequiredService<IConfiguration>();
    var configuration = builder.Configuration;

    var apiBaseUrl = configuration.GetValue<string>($"AppSettings:API_BASE_URL_{(IsDevelopmentEnvironment ? "DEV" : "PROD")}");

    //var apiBaseUrl = (IsDevelopmentEnvironment ? API_BASE_URL_DEV : API_BASE_URL_PROD);

    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new ArgumentException("API base URL not found in configuration.");
    }

    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    };

    return httpClient;
});

await builder.Build().RunAsync();
