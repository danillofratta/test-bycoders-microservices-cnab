using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Consumer.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Store?> FindByNameAsync(string name, CancellationToken ct) => _db.Stores.FirstOrDefaultAsync(s => s.Name == name, ct);

    public async Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        await _db.Transactions.AddAsync(transaction);        
    }
}
