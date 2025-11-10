using Cnab.Api.Domain.Queries;
using MediatR;

namespace Cnab.Api.Application.Transactions.Queries.GetAllTransactions;

public sealed record GetAllTransactionsQuery : IRequest<IEnumerable<GetAllTransactionsResponse>>;
