using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Cnab.Consumer.Domain.Services;
using MediatR;

namespace Cnab.Consumer.Application.Transactions.ProcessCnabLine;

public sealed class ProcessCnabLineHandler : IRequestHandler<ProcessCnabLineCommand>
{
    private readonly ICnabParser _parser;
    private readonly IStoreRepository _stores;
    private readonly IUnitOfWork _uow;

    public ProcessCnabLineHandler(ICnabParser parser, IStoreRepository stores, IUnitOfWork uow)
    {
        _parser = parser;
        _stores = stores;
        _uow = uow;
    }

    public async Task<Unit> Handle(ProcessCnabLineCommand request, CancellationToken ct)
    {
        var tx = _parser.Parse(request.Line, out var storeName, out var owner);
        var store = await _stores.FindByNameAsync(storeName, ct);
        if (store is null)
        {
            store = new Store(storeName, owner);
            await _stores.AddAsync(store, ct);
            await _uow.SaveChangesAsync(ct);
        }

        //typeof(Transaction).GetProperty("StoreId")!.SetValue(tx, store.Id);
        tx.SetIdStore(store.Id);
        tx.SetStore(store);
        await _stores.AddTransactionAsync(tx, ct);
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
