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
        var line = "5201903010000013200556418150633123****7687145607MARIA JOSEFINALOJA DO Ó - MATRIZ";
        var tx = p.Parse(line, out var store, out var owner);

        tx.Type.Should().Be(5);
        tx.Nature.Should().Be("LoanReceipt");
        tx.Value.Should().Be(132m);
        tx.SignedValue.Should().Be(+132m);
        owner.Should().Be("MARIA JOSEFINA");
        store.Should().Be("LOJA DO Ó - MATRIZ");
    }
}
