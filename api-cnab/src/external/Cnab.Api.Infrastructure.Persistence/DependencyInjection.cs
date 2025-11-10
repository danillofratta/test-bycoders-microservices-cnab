using Cnab.Consumer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Api.Infrastructure.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddReadPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("ReadDatabase") ??
                 "Host=localhost;Port=5432;Database=cnab_consumer;Username=admin;Password=root";
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(cs).UseSnakeCaseNamingConvention());

        // read repositories
        services.AddScoped<Cnab.Api.Domain.Repositories.IStoreRepository, Cnab.Api.Infrastructure.Persistence.Repositories.StoreRepository>();
        services.AddScoped<Cnab.Api.Domain.Repositories.ITransactionRepository, Cnab.Api.Infrastructure.Persistence.Repositories.TransactionRepository>();

        return services;
    }
}
