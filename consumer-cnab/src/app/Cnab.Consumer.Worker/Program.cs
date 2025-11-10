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

// Apply migrations automatically with enhanced error handling
try 
{
    using (var scope = host.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        Console.WriteLine("🔄 Checking database connection...");
        
        // Test database connection
        if (db.Database.CanConnect())
        {
            Console.WriteLine("✅ Database connection successful!");
            
            // Ensure database is created
            db.Database.EnsureCreated();
            
            // Apply pending migrations if any
            var pendingMigrations = db.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"🔄 Applying {pendingMigrations.Count} pending migrations...");
                db.Database.Migrate();
                Console.WriteLine("✅ Migrations applied successfully!");
            }
            else 
            {
                Console.WriteLine("✅ Database is up to date!");
            }
        }
        else 
        {
            Console.WriteLine("⚠️ Cannot connect to database. Waiting for initialization...");
            // Wait a bit and try again
            Thread.Sleep(5000);
            db.Database.EnsureCreated();
            db.Database.Migrate();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error during database initialization: {ex.Message}");
    Console.WriteLine("The application will continue and rely on database init scripts...");
}

Console.WriteLine("🚀 Starting CNAB Consumer...");
await host.RunAsync();
