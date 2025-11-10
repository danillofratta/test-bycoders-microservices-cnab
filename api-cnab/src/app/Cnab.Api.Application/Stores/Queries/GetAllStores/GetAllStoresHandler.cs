using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using Cnab.Api.Domain.Queries;
using Cnab.Api.Domain.Repositories;
using MediatR;

namespace Cnab.Api.Application.Stores.Queries.GetAllStores;

public sealed class GetAllStoresHandler : IRequestHandler<GetAllStoresQuery, IEnumerable<GetAllStoresResponse>>
{
    private readonly IStoreRepository _repo;

    public GetAllStoresHandler(IStoreRepository repo) => _repo = repo;

    public async Task<IEnumerable<GetAllStoresResponse>> Handle(GetAllStoresQuery request, CancellationToken ct)
    {        
        var list = await _repo.GetAllStoresAsync(ct);
        return list.Select(t => new GetAllStoresResponse(t.Name, t.Owner, t.Balance));
    }
}
