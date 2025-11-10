using Cnab.Api.Application.Stores.Queries.GetAllStores;
using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using Cnab.Api.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiCnab.Tests.Presentation;
public class StoresControllerTests
{
    [Fact]
    public async Task GetStores_Returns_Ok_With_List()
    {
        var mockSender = new Mock<ISender>();
        mockSender.Setup(s => s.Send(It.IsAny<GetAllStoresQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetAllStoresResponse>());

        var controller = new StoresController(mockSender.Object);
        var result = await controller.GetStores(CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetTransactions_Returns_Error()
    {
        var mockSender = new Mock<ISender>();
        mockSender.Setup(s => s.Send(It.IsAny<GetAllStoresQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.ArgumentException("erro"));

        var controller = new StoresController(mockSender.Object);
        var result = await controller.GetStores(CancellationToken.None);

        var badrequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badrequest.StatusCode);
    }
}
