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

            // Register services (replace with your specific service registrations)
            builder.Services.AddControllers(); // Enables controllers for API endpoints

            // Register ISpendService and its implementation
            builder.Services.AddScoped<ISpendService, SpendService>();

            var app = builder.Build();

            app.MapControllers(); // Maps controllers to the request pipeline

            app.Run();
        }
    }
}