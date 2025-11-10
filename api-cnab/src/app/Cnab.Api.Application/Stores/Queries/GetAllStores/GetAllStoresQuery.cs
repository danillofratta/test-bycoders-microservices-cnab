using Cnab.Api.Domain.Queries;
using MediatR;

namespace Cnab.Api.Application.Stores.Queries.GetAllStores;

public sealed record GetAllStoresQuery : IRequest<IEnumerable<GetAllStoresResponse>>;
