using FluentAssertions;
using Xunit;
using Cnab.Api.Domain.Entities;

namespace ApiCnab.Tests.Domain;
public class TransactionTests
{
    [Fact]
    public void Create_Constructs_Transaction_With_Properties()
    {
        var when = System.DateTime.UtcNow;
        var tx = Transaction.Create(1, "Income", 100m, 100m, "12345678901", "1234", when, "Store", "Owner");

        tx.Type.Should().Be(1);
        tx.StoreName.Should().Be("Store");
        tx.StoreOwner.Should().Be("Owner");
    }
}
