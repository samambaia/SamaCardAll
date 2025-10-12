using FrontWeb;
using FrontWeb.Handlers;
using FrontWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<TokenService>();
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<LoadingService>();

builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<TokenRefreshHandler>();

builder.Services.AddRadzenComponents();

builder.Services.AddHttpClient("ApiWithHandlers")
    // 🚨 1. ADICIONE O REFRESH HANDLER (CHAMA O REFRESH EM CASO DE 401)
    .AddHttpMessageHandler<TokenRefreshHandler>()
    // 2. ADICIONE O AUTH HEADER HANDLER (ADICIONA O TOKEN ATUAL NO HEADER)
    .AddHttpMessageHandler<AuthHeaderHandler>();

// Load configuration from appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

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

builder.Services.AddScoped<HttpClient>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();

    // Cria o cliente usando a definição "ApiWithHandlers" (que inclui os Handlers)
    var client = factory.CreateClient("ApiWithHandlers");

    // Aplica o BaseAddress correto, carregado do appsettings.json
    client.BaseAddress = new Uri(apiBaseUrl!);

    return client;
});

// --- Adicione esta seção para definir a cultura globalmente ---
var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
// --- Fim da seção de cultura ---

await builder.Build().RunAsync();
