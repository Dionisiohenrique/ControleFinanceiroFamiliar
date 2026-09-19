using ControleFinanceiroFamiliar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroFamiliar.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Family> Families => Set<Family>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Family>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Name).IsRequired().HasMaxLength(120);
                entity.Property(c => c.Currency).HasMaxLength(3).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Name).HasMaxLength(120).IsRequired();
                e.Property(x => x.Email).HasMaxLength(200).IsRequired();
                e.Property(x => x.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Color).HasMaxLength(9);
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Amount).HasPrecision(14, 2);
                e.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
                e.HasOne(x => x.Member).WithMany().HasForeignKey(x => x.MemberId);
                e.HasIndex(x => new { x.FamilyId, x.Date });
            });
        }
    }
}
