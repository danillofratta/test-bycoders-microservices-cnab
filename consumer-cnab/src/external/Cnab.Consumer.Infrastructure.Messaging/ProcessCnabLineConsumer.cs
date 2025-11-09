using Cnab.Consumer.Application.Transactions.ProcessCnabLine;
using Cnab.Domain.Events;
using MassTransit;
using MediatR;
namespace Cnab.Consumer.Infrastructure.Messaging;
public sealed class ProcessCnabLineConsumer : IConsumer<CnabLineMessageEvent>
{
    private readonly ISender _sender;
    public ProcessCnabLineConsumer(ISender sender){ _sender = sender; }
    public async Task Consume(ConsumeContext<CnabLineMessageEvent> context)
        => await _sender.Send(new ProcessCnabLineCommand(context.Message.Line));
}
