using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cnab.Api.Application.Transactions.Command.PublishCnabFile;

public sealed record PublishCnabFileCommand(IFormFile File) : IRequest<int>;
