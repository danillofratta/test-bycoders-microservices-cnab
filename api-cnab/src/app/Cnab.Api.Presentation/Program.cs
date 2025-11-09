using Cnab.Api.Application.Transactions.PublishCnabFile;
using Cnab.Api.Domain.Services;
using Cnab.Api.Infrastructure.Messaging;
using Cnab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.OperationFilter<FileUploadOperationFilter>());
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddReadPersistence(builder.Configuration);
builder.Services.AddSingleton<ICnabParser, CnabParser>();
builder.Services.AddMediatR(typeof(PublishCnabFileCommand).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapPost("/api/upload", async (HttpRequest req, ISender sender) =>
{
    // Guard against requests without form/multipart content-type
    if (!req.HasFormContentType || req.ContentLength == 0)
    {
        return Results.BadRequest("Request must be multipart/form-data with a file.");
    }

    var form = await req.ReadFormAsync();
    var file = form.Files.FirstOrDefault();
    if (file == null)
    {
        return Results.BadRequest("file is required");
    }

    var published = await sender.Send(new PublishCnabFileCommand(file));
    return Results.Ok(new { published });
})
.WithTags("CNAB");

app.MapGet("/api/stores", async (ReadDbContext db) =>
    await db.Stores
        .Select(s => new { s.Name, Balance = s.Transactions.Sum(t => t.SignedValue) })
        .ToListAsync()
).WithTags("CNAB");

app.Run();
