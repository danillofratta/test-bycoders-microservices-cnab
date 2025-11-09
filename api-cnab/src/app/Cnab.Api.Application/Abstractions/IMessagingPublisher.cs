namespace Cnab.Api.Application.Abstractions;

public interface IMessagingPublisher
{
    Task PublishAsync<T>(T message, CancellationToken ct);
}
