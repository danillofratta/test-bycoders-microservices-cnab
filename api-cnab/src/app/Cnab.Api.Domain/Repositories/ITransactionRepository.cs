



using Cnab.Api.Domain.Entities;

namespace Cnab.Api.Domain.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAll(CancellationToken ct);
}
