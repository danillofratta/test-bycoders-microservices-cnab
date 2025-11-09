using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Cnab.Consumer.Infrastructure.Messaging;
public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddMassTransit(x => {
            x.AddConsumer<ProcessCnabLineConsumer>();
            x.UsingRabbitMq((context, cfgMq) => 
            {
                cfgMq.Host(new Uri("amqp://guest:guest@localhost:5672/"));

                cfgMq.ReceiveEndpoint("cnab-lines", e => {
                    e.ConfigureConsumer<ProcessCnabLineConsumer>(context);
                });
            });
        });
        return services;
    }
}
