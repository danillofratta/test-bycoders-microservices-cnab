using Cnab.Consumer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Cnab.Consumer.Application.Abstractions;

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
            e.Property(t => t.OccurredAt)
                .HasColumnType("timestamp with time zone")  
                .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
    );
        });
    }
}
