using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using MediatR;
using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Microsoft.AspNetCore.Mvc;
using Cnab.Api.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ApiCnab.Tests.Presentation;
public class TransactionsControllerTests
{
    [Fact]
    public async Task GetTransactions_Returns_Ok()
    {
        var mockSender = new Mock<ISender>();
        mockSender.Setup(s => s.Send(It.IsAny<GetAllTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetAllTransactionsResponse>());

        var controller = new TransactionsController(mockSender.Object);
        var result = await controller.GetTransactions(CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetTransactions_Returns_Error()
    {
        var mockSender = new Mock<ISender>();
        mockSender.Setup(s => s.Send(It.IsAny<GetAllTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.ArgumentException("erro"));

        var controller = new TransactionsController(mockSender.Object);
        var result = await controller.GetTransactions(CancellationToken.None);

        var badrequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badrequest.StatusCode);
    }

    [Fact]
    public async Task Post_With_File_Returns_Ok()
    {
        var mockSender = new Mock<ISender>();
        mockSender.Setup(s => s.Send(It.IsAny<PublishCnabFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var controller = new TransactionsController(mockSender.Object);
        // setup HttpContext with multipart form and file
        var context = new DefaultHttpContext();

        var content = "l1\n l2\n";
        var bytes = Encoding.UTF8.GetBytes(content);
        var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", "cnab.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        var files = new FormFileCollection { file };
        var form = new FormCollection(new Dictionary<string, StringValues>(), files);
        context.Request.Form = form;
        context.Request.Headers["Content-Type"] = "multipart/form-data; boundary=---";

        controller.ControllerContext = new ControllerContext { HttpContext = context };

        var result = await controller.Post();

        var ok = Assert.IsType<OkObjectResult>(result);
        ok.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task Post_Without_File_Returns_BadRequest()
    {
        var mockSender = new Mock<ISender>();
        var controller = new TransactionsController(mockSender.Object);
        var context = new DefaultHttpContext();
        // empty form
        var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection());
        context.Request.Form = form;
        context.Request.Headers["Content-Type"] = "multipart/form-data; boundary=---";
        controller.ControllerContext = new ControllerContext { HttpContext = context };

        var result = await controller.Post();
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        bad.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
