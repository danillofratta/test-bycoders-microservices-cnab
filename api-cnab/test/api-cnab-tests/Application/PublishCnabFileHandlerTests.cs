using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cnab.Api.Application.Abstractions;
using Cnab.Api.Application.Transactions.PublishCnabFile;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Xunit;

namespace ApiCnab.Tests.Application;
public class PublishCnabFileHandlerTests
{
    //[Fact]
    //public async Task Should_Publish_One_Message_Per_Line()
    //{
    //    var mock = new Mock<IMessagingPublisher>();
    //    var handler = new PublishCnabFileHandler(mock.Object);
    //    var content = "l1\n\n l2 \n";
    //    var bytes = Encoding.UTF8.GetBytes(content);
    //    var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", "cnab.txt");

    //    var count = await handler.Handle(new PublishCnabFileCommand(file), default);

    //    count.Should().Be(2);
    //    mock.Verify(m => m.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    //}
}
