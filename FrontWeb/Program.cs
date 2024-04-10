using FrontWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using SamaCardAll.Infra;

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

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Configure the database provider and connection string here
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

await builder.Build().RunAsync();
