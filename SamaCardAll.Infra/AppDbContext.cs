using Microsoft.EntityFrameworkCore;
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
                .HasForeignKey(rt => rt.UserIdUser)
                .OnDelete(DeleteBehavior.Cascade);

            // 🚨 CORREÇÃO PARA O ERRO 's.UserId' NO SPEND REPOSITORY
            // Força o EF Core a usar a propriedade UserIdUser como chave estrangeira.
            modelBuilder.Entity<Spend>()
                .HasOne(s => s.User)
                .WithMany() // Se User não tem uma lista de Spends, use WithMany()
                .HasForeignKey(s => s.UserIdUser) // ⬅️ Usa a propriedade UserIdUser
                .IsRequired() // Garante que a coluna não seja nula
                .OnDelete(DeleteBehavior.Restrict); // Escolha o comportamento de deleção apropriado

            // 🚨 ADICIONAL: Garante que o nome da coluna no banco é exatamente 'UserIdUser'
            modelBuilder.Entity<Spend>()
                .Property(s => s.UserIdUser)
                .HasColumnName("UserIdUser");
        }
    }
}
