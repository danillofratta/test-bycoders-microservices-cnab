using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Infrastructure.Persistence;
using Cnab.Consumer.Infrastructure.Persistence.Repositories;

namespace Cnab.Consumer.Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        TransactionRepository = new TransactionRepository(_context);        
    }

    public ITransactionRepository TransactionRepository { get; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}