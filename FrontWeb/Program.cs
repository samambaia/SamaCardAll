using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

// Load configuration from appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

// Load appsettings.json manually
using var configStream = await httpClient.GetStreamAsync("appsettings.json");
using var jsonDoc = await JsonDocument.ParseAsync(configStream);
var root = jsonDoc.RootElement;

// Detect environment from launchsettings.json
var envName = builder.HostEnvironment.Environment ?? "Development"; // "Development", "Production", etc.
Console.WriteLine($"[INFO] Ambiente detectado: {envName}");

var configKey = envName == "Development" ? "DEV" : "PROD";

// 🔧 Reads API URL based on environment
var apiBaseUrl = root
    .GetProperty("ApiSettings")
    .GetProperty(configKey)
    .GetProperty("BaseUrl")
    .GetString();

Console.WriteLine($"[DEBUG] API URL configurada: {apiBaseUrl}");

// Register HttpClient with this URL
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl!) });

// --- Adicione esta seção para definir a cultura globalmente ---
var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
// --- Fim da seção de cultura ---

await builder.Build().RunAsync();
