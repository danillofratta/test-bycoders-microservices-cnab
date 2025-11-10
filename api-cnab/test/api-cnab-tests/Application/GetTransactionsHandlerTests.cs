using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using Cnab.Api.Domain.Repositories;
using Cnab.Api.Domain.Queries;
using Cnab.Api.Domain;
using Cnab.Api.Domain.Entities;

namespace ApiCnab.Tests.Application;
public class GetTransactionsHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Transactions_From_Repository()
    {
        var mockRepo = new Mock<ITransactionRepository>();
        var transactions = new List<Transaction>
        {
            new Transaction(1, "Income", 100m, 100m, "123", "card1", System.DateTime.UtcNow, "Store A", "Owner A")
        };
        mockRepo.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(transactions);

        var handler = new GetAllTransactionsHandler(mockRepo.Object);
        var result = await handler.Handle(new GetAllTransactionsQuery(), CancellationToken.None);

        result.Should().HaveCount(1);
        mockRepo.Verify(r => r.GetAll(It.IsAny<CancellationToken>()), Times.Once);
    }
}
