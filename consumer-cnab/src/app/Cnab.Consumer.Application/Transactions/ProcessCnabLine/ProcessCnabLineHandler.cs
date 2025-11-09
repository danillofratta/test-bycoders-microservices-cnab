using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Cnab.Consumer.Domain.Services;
using Cnab.Consumer.Infrastructure.Persistence;
using MediatR;

namespace Cnab.Consumer.Application.Transactions.ProcessCnabLine;

public sealed class ProcessCnabLineHandler : IRequestHandler<ProcessCnabLineCommand>
{
    private readonly ICnabParser _parser;
    private readonly IStoreRepository _storeRepo;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IUnitOfWork _uow;

    public ProcessCnabLineHandler(ICnabParser parser, ITransactionRepository transactionRepo, IStoreRepository storerepo, IUnitOfWork uow)
    {
        _parser = parser;
        _storeRepo = storerepo;
        _transactionRepo = transactionRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(ProcessCnabLineCommand request, CancellationToken ct)
    {
        var tx = _parser.Parse(request.Line, out var storeName, out var owner);
        var store = await _storeRepo.FindByNameAsync(storeName, ct);
        if (store is null)
        {
            store = new Store(storeName, owner);
            await _storeRepo.AddAsync(store, ct);            
        }
        
        tx.SetIdStore(store.Id);
        tx.SetStore(store);

        await _transactionRepo.AddAsync(tx, ct);
        await _uow.CompleteAsync();

        return Unit.Value;
    }
}
