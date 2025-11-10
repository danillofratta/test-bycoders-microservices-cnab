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

    public async Task<bool> ExistsAsync(
        int type, decimal value, string cpf, string card,
        DateTime occurredAt, string storeName, string storeOwner,
        CancellationToken ct)
    {
        return await _db.Transactions.AnyAsync(t =>
            t.Type == type &&
            t.Value == value &&
            t.Cpf == cpf &&
            t.Card == card &&
            t.OccurredAt == occurredAt.ToUniversalTime() &&
            t.StoreName == storeName &&
            t.StoreOwner == storeOwner, ct);
    }
}
