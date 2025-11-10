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
            });
        });

        return services;
    }
}
