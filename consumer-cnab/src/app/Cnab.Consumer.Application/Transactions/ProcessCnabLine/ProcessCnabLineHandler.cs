using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Cnab.Consumer.Domain.Services;
using Cnab.Consumer.Infrastructure.Persistence;
using MediatR;

namespace Cnab.Consumer.Application.Transactions.ProcessCnabLine;

public sealed class ProcessCnabLineHandler : IRequestHandler<ProcessCnabLineCommand>
{
    private readonly ICnabParser _parser;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessCnabLineHandler(ICnabParser parser, ITransactionRepository transactionRepo, IUnitOfWork unitOfWork)
    {
        _parser = parser;
        _transactionRepo = transactionRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ProcessCnabLineCommand request, CancellationToken ct)
    {
        var tx = _parser.Parse(request.Line, out var storeName, out var owner);

        bool exists = await _transactionRepo.ExistsAsync(
            tx.Type, tx.Value, tx.Cpf, tx.Card,
            tx.OccurredAt, tx.StoreName, tx.StoreOwner, ct);

        if (exists)
            return Unit.Value;

        await _transactionRepo.AddAsync(tx, ct);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
