using System;
using System.Threading.Tasks;
using Cnab.Consumer.Application.Abstractions;
using Cnab.Consumer.Application.Transactions.ProcessCnabLine;
using Cnab.Consumer.Domain.Services;
using Cnab.Consumer.Infrastructure.Persistence;
using Cnab.Consumer.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConsumerCnab.Tests.Application;
public class ProcessCnabLineHandlerTests
{
    [Fact]
    public async Task Should_Create_Store_And_Persist_Transaction()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase($"db_{Guid.NewGuid()}").Options;
        var db = new AppDbContext(options);
        var repo = new StoreRepository(db);
        IUnitOfWork uow = db;
        var handler = new ProcessCnabLineHandler(new CnabParser(), repo, uow);

        var line = "1201903010000014200096206760174753****3153 153000   JOÃO MACEDO    MERCADO DA AVENIDA  ";
        await handler.Handle(new ProcessCnabLineCommand(line), default);

        (await db.Stores.CountAsync()).Should().Be(1);
        (await db.Transactions.CountAsync()).Should().Be(1);
    }
}
