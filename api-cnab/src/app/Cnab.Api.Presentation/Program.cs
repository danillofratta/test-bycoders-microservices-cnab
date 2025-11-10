using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Cnab.Api.Infrastructure.Messaging;
using Cnab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cnab API", Version = "v1" });
    c.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); 
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddMediatR(typeof(PublishCnabFileCommand).Assembly);

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Ensure database is created and migrations are applied
try 
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<Cnab.Consumer.Infrastructure.Persistence.AppDbContext>();
        
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Apply pending migrations
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        
        Console.WriteLine("✅ Database initialized successfully!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error initializing database: {ex.Message}");
    Console.WriteLine("Continuing startup - database will be handled by init script...");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cnab API v1"));
}

app.UseCors();

app.MapControllers();

app.Run();
