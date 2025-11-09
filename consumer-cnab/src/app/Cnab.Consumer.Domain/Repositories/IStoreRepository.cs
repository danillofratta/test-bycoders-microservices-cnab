using Cnab.Consumer.Domain.Entities;

namespace Cnab.Consumer.Application.Abstractions;

public interface IStoreRepository
{
    Task<Store?> FindByNameAsync(string name, CancellationToken ct);
    Task AddAsync(Store store, CancellationToken ct);
}
