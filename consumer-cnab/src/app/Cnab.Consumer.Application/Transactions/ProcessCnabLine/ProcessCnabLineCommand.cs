using MediatR;

namespace Cnab.Consumer.Application.Transactions.ProcessCnabLine;

public sealed record ProcessCnabLineCommand(string Line) : IRequest;
