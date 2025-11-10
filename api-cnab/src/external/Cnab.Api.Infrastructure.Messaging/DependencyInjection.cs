using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Api.Infrastructure.Messaging;
public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
    {
        ArgumentNullException.ThrowIfNull(cfg);

        var uriString = cfg["Rabbit:Uri"];

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfgMq) =>
            {
                cfgMq.Host(uriString);

                // Configure a receive endpoint for consumers if this process runs consumers
                cfgMq.ReceiveEndpoint("cnab-lines", e =>
                {
                    e.ConfigureConsumers(context);
                });
            });
        });

        return services;
    }
}
