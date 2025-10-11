using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Entities;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Infra
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Spend> Spends { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Installments> Installments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
