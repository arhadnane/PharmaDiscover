using Microsoft.EntityFrameworkCore;
using MoleculeSimulator.Models;

namespace MoleculeSimulator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Molecule> Molecules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Molecule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MolecularWeight).HasPrecision(18, 2);
                entity.Property(e => e.LogP).HasPrecision(18, 2);
                entity.Property(e => e.Polarity).HasPrecision(18, 4);
                entity.Property(e => e.TherapeuticScore).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                // Index for better query performance
                entity.HasIndex(e => e.TherapeuticScore);
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}
