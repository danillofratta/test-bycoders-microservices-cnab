using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Cnab.Api.Infrastructure.Messaging;
using Cnab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cnab API", Version = "v1" });
    c.OperationFilter<FileUploadOperationFilter>();
});

// API versioning
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

// Cross-origin
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Infrastructure
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

// Application
builder.Services.AddMediatR(typeof(PublishCnabFileCommand).Assembly);

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<Cnab.Api.Infrastructure.Persistence.AppDbContext>();
        dbContext.Database.EnsureCreated();
        Console.WriteLine("Database initialized successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization failed: {ex.Message}");
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cnab API v1"));
}

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
