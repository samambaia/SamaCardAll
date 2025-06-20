using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra.Repository;

namespace SamaCardAll
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            builder.Logging.AddConsole();

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var constr = builder.Configuration.GetConnectionString("DefaultConnection");

            // Enables controllers for API endpoints
            builder.Services.AddControllers(); 

            // Register Services and its implementation
            builder.Services.AddScoped<ISpendService, SpendService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddScoped<IReportService, ReportService>();

            // Register Repositories and its implementation
            builder.Services.AddScoped<ISpendRepository, SpendRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();

            // Integrating Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SamaCard API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SamaCard v1");
                    c.RoutePrefix = string.Empty; // To serve Swagger UI at the root URL (http://localhost:<port>/)
                });
            }

            // Adiciona a configuração de CORS
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            /*
             * To use in Production Environment
             * app.UseCors(options =>
                {
                    options.WithOrigins("https://yourfrontenddomain.com")
                            .AllowMethods("GET", "POST")
                            .WithHeaders("Content-Type");
                });
            */

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Maps controllers to the request pipeline
            app.MapControllers(); 
            
            app.MapGet("/", () => "API is Running...");
            
            app.Run();
        }
    }
}