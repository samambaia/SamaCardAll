using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// If I want run in Dev environment, set for true
bool IsDevelopmentEnvironment = true;

string API_BASE_URL_DEV = "https://localhost:44383/";
string API_BASE_URL_PROD = "http://localhost:5000/";

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<HttpClient>(sp =>
{
    //var configuration = sp.GetRequiredService<IConfiguration>();
    //var apiBaseUrl = configuration.GetValue<string>(IsDevelopmentEnvironment ? "API_BASE_URL_DEV" : "API_BASE_URL_PROD");

    var apiBaseUrl = (IsDevelopmentEnvironment ? API_BASE_URL_DEV : API_BASE_URL_PROD);

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

//builder.Services.AddScoped(sp =>
//{
//    // URL base da sua API
//    var apiBaseUrl = "http://localhost:5000/"; 

//    // Configuração do HttpClient com a URL da API
//    var httpClient = new HttpClient
//    {
//        BaseAddress = new Uri(apiBaseUrl)
//    };

//    return httpClient;
//});

await builder.Build().RunAsync();
