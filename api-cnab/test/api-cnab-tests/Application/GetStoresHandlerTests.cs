using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Cnab.Api.Application.Stores.Queries.GetAllStores;
using Cnab.Api.Domain.Repositories;
using Cnab.Api.Domain.Queries;

namespace ApiCnab.Tests.Application;
public class GetStoresHandlerTests
{
    [Fact]
    public async Task Handle_Returns_StoreSummaries_From_Repository()
    {
        var mockRepo = new Mock<IStoreRepository>();
        var expected = new List<StoreSummary>
        {
            new StoreSummary("Name1", "Owner1", 100m),
            new StoreSummary("Name2", "Owner2", -50m)
        };
        mockRepo.Setup(r => r.GetAllStoresAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var handler = new GetAllStoresHandler(mockRepo.Object);
        var result = await handler.Handle(new GetAllStoresQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(new[] { new GetAllStoresResponse("Name1","Owner1",100m), new GetAllStoresResponse("Name2","Owner2",-50m) });
        mockRepo.Verify(r => r.GetAllStoresAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
