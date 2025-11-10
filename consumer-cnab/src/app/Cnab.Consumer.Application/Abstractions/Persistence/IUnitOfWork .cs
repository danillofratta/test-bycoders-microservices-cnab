using Cnab.Consumer.Application.Abstractions;


namespace Cnab.Consumer.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{    
    ITransactionRepository TransactionRepository { get; }
    Task<int> CompleteAsync();
}