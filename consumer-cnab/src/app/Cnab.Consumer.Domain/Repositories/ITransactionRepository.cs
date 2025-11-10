using Cnab.Consumer.Domain.Entities;

namespace Cnab.Consumer.Application.Abstractions;

public interface ITransactionRepository
{    Task AddAsync(Transaction transaction, CancellationToken ct);
    Task<bool> ExistsAsync(int type, decimal value, string cpf, string card,
        DateTime occurredAt, string storeName, string storeOwner,
        CancellationToken ct);
}
