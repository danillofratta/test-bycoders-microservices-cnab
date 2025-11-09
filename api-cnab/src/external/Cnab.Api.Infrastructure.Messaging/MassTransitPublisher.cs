using Cnab.Api.Application.Abstractions;
using MassTransit;
namespace Cnab.Api.Infrastructure.Messaging;
public sealed class MassTransitPublisher : IMessagingPublisher
{
    private readonly IPublishEndpoint _publish;
    public MassTransitPublisher(IPublishEndpoint publish) => _publish = publish;
    public Task PublishAsync<T>(T message, CancellationToken ct) => _publish.Publish(message, ct);
}
