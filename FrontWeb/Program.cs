using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

// Load configuration from appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var appSettingsJson = await httpClient.GetStringAsync("appsettings.json");
var appSettings = JsonSerializer.Deserialize<AppSettings>(appSettingsJson);

// Determine the environment and set the API base URL accordingly
var isDevelopment = builder.Configuration.GetValue<bool>("IsDevelopment");
var apiBaseUrl = isDevelopment ? appSettings.Environment_DEV.ApiBaseUrl : appSettings.Environment_PROD.ApiBaseUrl;
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

await builder.Build().RunAsync();

public class EnvironmentConfig
{
    public string ApiBaseUrl { get; set; }
}
public class AppSettings
{
    public bool IsDevelopment { get; set; }
    public EnvironmentConfig Environment_DEV { get; set; }
    public EnvironmentConfig Environment_PROD { get; set; }
}