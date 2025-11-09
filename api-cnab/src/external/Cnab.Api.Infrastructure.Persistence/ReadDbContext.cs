using Cnab.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configure mappings se necessário
        base.OnModelCreating(modelBuilder);
    }
}
