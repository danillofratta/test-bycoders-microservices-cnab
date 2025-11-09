using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Infrastructure.Persistence;
using Cnab.Consumer.Infrastructure.Persistence.Repositories;

namespace Cnab.Consumer.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IStoreRepository StoreRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    Task<int> CompleteAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        StoreRepository = new StoreRepository(_context);
        TransactionRepository = new TransactionRepository(_context);        
    }

    public IStoreRepository StoreRepository { get; }
    public ITransactionRepository TransactionRepository { get; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}