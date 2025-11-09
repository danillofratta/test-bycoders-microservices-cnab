using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnab.Api.Infrastructure.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddReadPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("ReadDatabase") ??
                 "Host=postgres;Port=5432;Database=cnab_consumer;Username=admin;Password=root";
        services.AddDbContext<ReadDbContext>(o=>o.UseNpgsql(cs).UseSnakeCaseNamingConvention());
        return services;
    }
}
