using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Consumer.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _db;

    public StoreRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Store?> FindByNameAsync(string name, CancellationToken ct) => _db.Stores.FirstOrDefaultAsync(s => s.Name == name, ct);

    public Task AddAsync(Store store, CancellationToken ct)
    {
        _db.Stores.Add(store);
        return Task.CompletedTask;
    }

    public Task AddTransactionAsync(Transaction tx, CancellationToken ct)
    {
        _db.Transactions.Add(tx);
        return Task.CompletedTask;
    }
}
