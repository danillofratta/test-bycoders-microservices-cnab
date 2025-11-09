using Cnab.Consumer.Domain.Entities;

namespace Cnab.Consumer.Domain.Services;

public interface ICnabParser
{
    Transaction Parse(string line, out string store, out string owner);
}
