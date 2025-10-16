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

// TokenRefreshHandler precisa ser Scoped
builder.Services.AddScoped<TokenRefreshHandler>();

builder.Services.AddRadzenComponents();

// --- NOVO: 1. CLIENTE AUTH (SEM HANDLERS) ---
// Este é o cliente usado pelo CustomAuthenticationStateProvider para o refresh token.
builder.Services.AddHttpClient("ApiAuth");

// 2. Cliente principal COM Handlers
builder.Services.AddHttpClient("ApiWithHandlers")
    // O TokenRefreshHandler (que faz o bypass) precisa vir antes do AuthHeaderHandler.
    .AddHttpMessageHandler<TokenRefreshHandler>()
    .AddHttpMessageHandler<AuthHeaderHandler>();

var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

using var configStream = await httpClient.GetStreamAsync("appsettings.json");
using var jsonDoc = await JsonDocument.ParseAsync(configStream);
var root = jsonDoc.RootElement;

var envName = builder.HostEnvironment.Environment ?? "Development";
Console.WriteLine($"[INFO] Ambiente detectado: {envName}");

var configKey = envName == "Development" ? "DEV" : "PROD";

var apiBaseUrl = root
    .GetProperty("ApiSettings")
    .GetProperty(configKey)
    .GetProperty("BaseUrl")
    .GetString();

Console.WriteLine($"[DEBUG] API URL configurada: {apiBaseUrl}");


// --- CONFIGURAÇÃO FINAL DO HTTPCLIENT ---
builder.Services.AddScoped<HttpClient>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();

    // 1. Configura e armazena o BaseAddress para o cliente padrão (ApiWithHandlers)
    var client = factory.CreateClient("ApiWithHandlers");
    client.BaseAddress = new Uri(apiBaseUrl!);

    // 2. Configura e armazena o BaseAddress para o cliente de Autenticação (ApiAuth)
    // Isso garante que o CustomAuthenticationStateProvider use a URL correta no refresh.
    var authClient = factory.CreateClient("ApiAuth");
    authClient.BaseAddress = new Uri(apiBaseUrl!);

    // Retorna o cliente padrão (com handlers) para a injeção global de HttpClient
    return client;
});

// --- Adicione esta seção para definir a cultura globalmente ---
var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
// --- Fim da seção de cultura ---

await builder.Build().RunAsync();