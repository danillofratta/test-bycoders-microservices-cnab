using Cnab.Domain.Events;
using MassTransit;
using MediatR;

namespace Cnab.Api.Application.Transactions.Command.PublishCnabFile;

public sealed class PublishCnabFileHandler : IRequestHandler<PublishCnabFileCommand, int>
{
    private readonly IPublishEndpoint _publisher;   

    public PublishCnabFileHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task<int> Handle(PublishCnabFileCommand request, CancellationToken ct)
    {
        using var stream = request.File.OpenReadStream();
        using var reader = new StreamReader(stream);

        string? line;
        int count = 0;

        while ((line = await reader.ReadLineAsync()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            await _publisher.Publish(new CnabLineMessageEvent(line), ct);
            count++;
        }

        return count;
    }
}

