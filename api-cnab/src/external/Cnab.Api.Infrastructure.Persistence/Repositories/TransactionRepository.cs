using Cnab.Api.Domain;
using Cnab.Api.Domain.Entities;
using Cnab.Api.Domain.Repositories;
using Cnab.Consumer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Api.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Transaction>> GetAll(CancellationToken ct)
    {
        return await _db.Transactions.ToListAsync(ct);
    }
}
