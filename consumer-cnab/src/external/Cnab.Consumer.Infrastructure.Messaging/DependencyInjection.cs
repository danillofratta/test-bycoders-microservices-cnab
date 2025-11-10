using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Cnab.Consumer.Infrastructure.Messaging;
public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
    {
        var uriString = cfg["Rabbit:Uri"];

        services.AddMassTransit(x => {
            x.AddConsumer<ProcessCnabLineConsumer>();
            x.UsingRabbitMq((context, cfgMq) => 
            {
                cfgMq.Host(new Uri(uriString));

                cfgMq.ReceiveEndpoint("cnab-line-queue", e => {
                    e.ConfigureConsumer<ProcessCnabLineConsumer>(context);
                });
            });
        });
        return services;
    }
}
