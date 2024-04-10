using Microsoft.EntityFrameworkCore;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Spend> Spends { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Installments> Installments { get; set; }
    }
}
