using Cnab.Api.Domain.Queries;

namespace Cnab.Api.Domain.Repositories;

public interface IStoreRepository
{
    Task<IEnumerable<StoreSummary>> GetAllStoresAsync(CancellationToken ct);
}
