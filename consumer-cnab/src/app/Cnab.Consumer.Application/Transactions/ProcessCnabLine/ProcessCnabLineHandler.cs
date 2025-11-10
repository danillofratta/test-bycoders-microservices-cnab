using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Domain.Entities;
using Cnab.Consumer.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cnab.Consumer.Application.Transactions.ProcessCnabLine;

public sealed class ProcessCnabLineHandler : IRequestHandler<ProcessCnabLineCommand>
{
    private readonly ICnabParser _parser;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessCnabLineHandler> _logger;

    public ProcessCnabLineHandler(ICnabParser parser, ITransactionRepository transactionRepo, IUnitOfWork unitOfWork, ILogger<ProcessCnabLineHandler> logger)
    {
        _parser = parser;
        _transactionRepo = transactionRepo;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(ProcessCnabLineCommand request, CancellationToken ct)
    {
        _logger.LogDebug("Handling CNAB line (prefix): {Prefix}", request.Line is null ? string.Empty : request.Line.Length <= 30 ? request.Line : request.Line.Substring(0, 30));

        var tx = _parser.Parse(request.Line, out var storeName, out var owner);
        _logger.LogDebug("Parsed transaction: Type={Type} Value={Value} Store={Store}", tx.Type, tx.Value, storeName);

        try
        {
            // existence check
            bool exists = await _transactionRepo.ExistsAsync(
                tx.Type, tx.Value, tx.Cpf, tx.Card,
                tx.OccurredAt, tx.StoreName, tx.StoreOwner, ct);

            if (exists)
            {
                _logger.LogInformation("Skipping duplicate transaction: Type={Type} Value={Value} Store={Store}", tx.Type, tx.Value, tx.StoreName);
                return Unit.Value;
            }

            // not exists, add
            await _transactionRepo.AddAsync(tx, ct);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Transaction persisted: Type={Type} Value={Value} Store={Store}", tx.Type, tx.Value, tx.StoreName);

            return Unit.Value;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            _logger.LogWarning("Processing cancelled for CNAB line: {Store}", storeName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing CNAB line for store {Store}", storeName);
            throw;
        }
    }
}
