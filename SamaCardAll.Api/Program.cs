using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using SamaCardAll.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services (replace with your specific service registrations)
        builder.Services.AddControllers(); // Enables controllers for API endpoints

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Optional: Configure middleware pipeline (if needed)

        app.MapControllers(); // Maps controllers to the request pipeline

        app.Run();
    }
}
