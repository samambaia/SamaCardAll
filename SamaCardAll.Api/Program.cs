using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.OpenApi.Models;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                ));

            var constr = builder.Configuration.GetConnectionString("DefaultConnection");

            // Enables controllers for API endpoints
            builder.Services.AddControllers();

            // Need to access HTTP Context (where token is)
            builder.Services.AddHttpContextAccessor();

            // Register Services and its implementation
            builder.Services.AddScoped<ISpendService, SpendService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();

            // Register Repositories and its implementation
            builder.Services.AddScoped<ISpendRepository, SpendRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 👇 VALIDAÇÃO: Verifica se o Secret foi carregado
                    var secret = builder.Configuration["Jwt:Secret"];
                    if (string.IsNullOrEmpty(secret))
                    {
                        throw new InvalidOperationException("A variável Jwt:Secret não foi configurada. Verifique appsettings.json ou variáveis de ambiente.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });


            builder.Services.AddAuthorization();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Integrating Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SamaCard API", Version = "v1" });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }); 
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

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            // Maps controllers to the request pipeline
            app.MapControllers(); 
            
            app.MapGet("/", () => "API is Running...")
                .ExcludeFromDescription(); // Exclude from Swagger documentation

            app.Run();
        }
    }
}