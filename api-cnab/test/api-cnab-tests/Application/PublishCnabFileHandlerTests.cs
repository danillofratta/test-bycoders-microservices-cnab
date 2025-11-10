using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Moq;
using Xunit;
using FluentAssertions;
using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Cnab.Domain.Events;
using Microsoft.AspNetCore.Http;

namespace ApiCnab.Tests.Application;
public class PublishCnabFileHandlerTests
{
    [Fact]
    public async Task Should_Publish_One_Message_Per_NonEmpty_Line()
    {
        var mockPublish = new Mock<IPublishEndpoint>();
        var handler = new PublishCnabFileHandler(mockPublish.Object);

        var content = "l1\n\n l2 \n";
        var bytes = Encoding.UTF8.GetBytes(content);

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(bytes));
        mockFile.Setup(f => f.Length).Returns(bytes.Length);
        mockFile.Setup(f => f.FileName).Returns("cnab.txt");

        var count = await handler.Handle(new PublishCnabFileCommand(mockFile.Object), CancellationToken.None);

        count.Should().Be(2);
        mockPublish.Verify(m => m.Publish(It.IsAny<CnabLineMessageEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}
