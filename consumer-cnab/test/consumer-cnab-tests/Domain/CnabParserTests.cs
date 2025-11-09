using System;
using Cnab.Consumer.Domain.Services;
using FluentAssertions;
using Xunit;

namespace ConsumerCnab.Tests.Domain;
public class CnabParserTests
{
    [Fact]
    public void Parse_Should_Handle_Expense_Type()
    {
        var p = new CnabParser();
        var line = "3201903010000010000096206760174753****3153 153000   JOÃO MACEDO    MERCADO DA AVENIDA  "; // type 3 => Expense
        var tx = p.Parse(line, out var store, out var owner);
        tx.Type.Should().Be(3);
        tx.Nature.Should().Be("Expense");
        tx.Value.Should().Be(10.00m);
        tx.SignedValue.Should().Be(-10.00m);
        store.Should().Be("MERCADO DA AVENIDA");
        owner.Should().Contain("JOÃO");
    }
}
