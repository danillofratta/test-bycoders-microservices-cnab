using FluentAssertions;
using Xunit;

namespace ApiCnab.Tests.Domain;
public class CnabParserTests
{
    //[Fact]
    //public void Parse_Should_Map_All_Fields()
    //{
    //    var p = new CnabParser();
    //    var line = "1201903010000014200096206760174753****3153 153000   JOÃO MACEDO    MERCADO DA AVENIDA  ";
    //    var tx = p.Parse(line, out var store, out var owner);

    //    tx.Type.Should().Be(1);
    //    tx.Nature.Should().Be("Income");
    //    tx.Value.Should().Be(142.00m);
    //    tx.SignedValue.Should().Be(142.00m);
    //    tx.Cpf.Should().Be("00096206760"); // observa: a string de exemplo pode conter zeros à esquerda
    //    tx.Card.Should().Contain("3153");
    //    store.Should().Be("MERCADO DA AVENIDA");
    //    owner.Should().Contain("JOÃO");
    //}
}
