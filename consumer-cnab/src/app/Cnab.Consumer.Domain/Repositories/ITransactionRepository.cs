using Cnab.Consumer.Domain.Entities;

namespace Cnab.Consumer.Application.Abstractions;

public interface ITransactionRepository
{    Task AddAsync(Transaction transaction, CancellationToken ct);
}
