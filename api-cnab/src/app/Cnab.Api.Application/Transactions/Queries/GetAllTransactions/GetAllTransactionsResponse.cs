
using System;
using Cnab.Api.Domain.Queries;
using MediatR;

namespace Cnab.Api.Application.Transactions.Queries.GetAllTransactions;

public sealed record GetAllTransactionsResponse(
    int Id,
    int Type,
    string Nature,
    decimal Value,  
    decimal SignedValue,
    string Cpf,
    string Card,
    DateTime OccurredAt,
    string StoreName,
    string StoreOwner
);


