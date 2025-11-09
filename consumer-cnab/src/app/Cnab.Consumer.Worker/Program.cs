using Cnab.Consumer.Application.Transactions.ProcessCnabLine;
using Cnab.Consumer.Domain.Services;
using Cnab.Consumer.Infrastructure.Messaging;
using Cnab.Consumer.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddPersistence(context.Configuration);
        services.AddMessaging(context.Configuration);
        services.AddSingleton<ICnabParser, CnabParser>();
        services.AddMediatR(typeof(ProcessCnabLineHandler).Assembly);
    })
    .Build();

// Apply migrations automatically
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

await host.RunAsync();
