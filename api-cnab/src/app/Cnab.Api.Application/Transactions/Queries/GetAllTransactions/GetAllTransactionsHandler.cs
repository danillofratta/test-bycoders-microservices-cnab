using Cnab.Api.Domain.Repositories;
using Cnab.Api.Domain.Queries;
using MediatR;

namespace Cnab.Api.Application.Transactions.Queries.GetAllTransactions;

public sealed class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, IEnumerable<GetAllTransactionsResponse>>
{
    private readonly ITransactionRepository _repo;

    public GetAllTransactionsHandler(ITransactionRepository repo) => _repo = repo;

    public async Task<IEnumerable<GetAllTransactionsResponse>> Handle(GetAllTransactionsQuery request, CancellationToken ct)
    {
        var list = await _repo.GetAll(ct);
        return list.Select(t => new GetAllTransactionsResponse(t.Id, t.Type, t.Nature, t.Value, t.SignedValue, t.Cpf, t.Card, t.OccurredAt, t.StoreName, t.StoreOwner));
    }
}
