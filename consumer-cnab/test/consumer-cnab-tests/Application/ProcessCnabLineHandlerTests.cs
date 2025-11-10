using System;
using System.Threading;
using System.Threading.Tasks;
using Cnab.Consumer.Application.Transactions.ProcessCnabLine;
using Cnab.Consumer.Domain.Entities;
using Cnab.Consumer.Domain.Services;
using Cnab.Consumer.Application.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace ConsumerCnab.Tests.Application;

public class ProcessCnabLineHandlerTests
{
    [Fact]
    public async Task Handle_When_Transaction_Exists_Should_Not_Add_Or_Complete()
    {
        // Arrange
        var line = "1201903010000014200096206760174753****3153 153000   JOÃO MACEDO    MERCADO DA AVENIDA  ";

        var tx = Transaction.Create(1, "Debit", 14.2m, 14.2m, "96206760174", "753****3153", DateTime.UtcNow, "MERCADO DA AVENIDA", "JOÃO MACEDO");

        var parserMock = new Mock<ICnabParser>();
        string outStore = tx.StoreName;
        string outOwner = tx.StoreOwner;
        parserMock.Setup(p => p.Parse(It.IsAny<string>(), out outStore, out outOwner)).Returns(tx);

        var txRepoMock = new Mock<Cnab.Consumer.Application.Abstractions.ITransactionRepository>();
        txRepoMock.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Mock the interface, not the concrete UnitOfWork implementation
        var uowMock = new Mock<Cnab.Consumer.Application.Abstractions.IUnitOfWork>();
        uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

        var loggerMock = new Mock<ILogger<ProcessCnabLineHandler>>();

        var handler = new ProcessCnabLineHandler(parserMock.Object, txRepoMock.Object, uowMock.Object, loggerMock.Object);

        // Act
        await handler.Handle(new ProcessCnabLineCommand(line), CancellationToken.None);

        // Assert
        txRepoMock.Verify(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        txRepoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
        uowMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_When_Transaction_Not_Exists_Should_Add_And_Complete()
    {
        // Arrange
        var line = "1201903010000014200096206760174753****3153 153000   JOÃO MACEDO    MERCADO DA AVENIDA  ";

        var tx = Transaction.Create(1, "Debit", 14.2m, 14.2m, "96206760174", "753****3153", DateTime.UtcNow, "MERCADO DA AVENIDA", "JOÃO MACEDO");

        var parserMock = new Mock<ICnabParser>();
        string outStore = tx.StoreName;
        string outOwner = tx.StoreOwner;
        parserMock.Setup(p => p.Parse(It.IsAny<string>(), out outStore, out outOwner)).Returns(tx);

        var txRepoMock = new Mock<Cnab.Consumer.Application.Abstractions.ITransactionRepository>();
        txRepoMock.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        txRepoMock.Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var uowMock = new Mock<Cnab.Consumer.Application.Abstractions.IUnitOfWork>();
        uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

        var loggerMock = new Mock<ILogger<ProcessCnabLineHandler>>();

        var handler = new ProcessCnabLineHandler(parserMock.Object, txRepoMock.Object, uowMock.Object, loggerMock.Object);

        // Act
        await handler.Handle(new ProcessCnabLineCommand(line), CancellationToken.None);

        // Assert
        txRepoMock.Verify(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        txRepoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
