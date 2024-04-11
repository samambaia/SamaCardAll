using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    // URL base da sua API
    var apiBaseUrl = "https://localhost:44383/";

    // Configuração do HttpClient com a URL da API
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    };

    return httpClient;
});

await builder.Build().RunAsync();
