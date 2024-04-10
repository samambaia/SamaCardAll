using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra;

namespace SamaCardAll
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Enables controllers for API endpoints
            builder.Services.AddControllers(); 

            // Register Services and its implementation
            builder.Services.AddScoped<ISpendService, SpendService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICardService, CardService>();

            var app = builder.Build();

            // Adiciona a configuração de CORS
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            // Maps controllers to the request pipeline
            app.MapControllers(); 
            
            app.MapGet("/", () => "API is Running...");
            
            app.Run();
        }
    }
}