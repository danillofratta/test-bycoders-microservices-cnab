using Cnab.Api.Domain.Queries;
using Cnab.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Cnab.Api.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _db;
    public StoreRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<StoreSummary>> GetAllStoresAsync(CancellationToken ct)
    {
        return await _db.Transactions
            .GroupBy(t => new { t.StoreName, t.StoreOwner })
            .Select(g => new StoreSummary
            (
                g.Key.StoreName,
                g.Key.StoreOwner,
                g.Sum(t => t.SignedValue)
            ))            
            .ToListAsync(ct);
    }
}
