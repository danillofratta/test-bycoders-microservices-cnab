using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Cnab.Consumer.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    /// <summary>
    /// Apply pending EF Core migrations using an IServiceProvider. This avoids adding a Hosting dependency to the persistence project.
    /// </summary>
    public static async Task MigrateDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var db = provider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync(cancellationToken);
    }
}
