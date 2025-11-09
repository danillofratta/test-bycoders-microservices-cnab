using Cnab.Consumer.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Consumer.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Postgres") ?? "Host=localhost;Port=5432;Database=cnab_consumer;Username=admin;Password=root";
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(cs).UseSnakeCaseNamingConvention());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<Repositories.StoreRepository>();
        services.AddScoped<IStoreRepository>(sp => sp.GetRequiredService<Repositories.StoreRepository>());
        return services;
    }
}
