using Microsoft.EntityFrameworkCore;
using Cnab.Api.Domain;
using Cnab.Api.Domain.Entities;

namespace Cnab.Consumer.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        

        modelBuilder.Entity<Transaction>(e =>
        {
            e.ToTable("transactions");
            e.HasKey(x => x.Id);
            e.Property(p => p.Nature)
                .HasMaxLength(20)
                .IsRequired();
            e.Property(p => p.Cpf)
                .HasMaxLength(11)
                .IsRequired();
            e.Property(p => p.Card)
                .HasMaxLength(12)
                .IsRequired();
        });
    }
}
