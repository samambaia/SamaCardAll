using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // 🚨 Este using é CRUCIAL!
using SamaCardAll.Infra;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 🚨 Connection String Fictícia Mínima
        // Não inclua senha para evitar erros de parsing.
        var dummyConnectionString = "Server=localhost;Database=MigrationDesignDb;User ID=root";

        // 🚨 Versão Fixada para EVITAR CONEXÃO
        // O Pomelo aceita a versão no formato de objeto
        // Assumindo que você está usando MySQL 8.0 no RDS
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            dummyConnectionString,
            serverVersion,
            o => o.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}