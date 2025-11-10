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
    public async Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        await _db.Transactions.AddAsync(transaction);        
    }
}
