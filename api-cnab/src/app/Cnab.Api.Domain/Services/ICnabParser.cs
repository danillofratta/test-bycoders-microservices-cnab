using Cnab.Api.Domain.Entities;
namespace Cnab.Api.Domain.Services;
public interface ICnabParser { Transaction Parse(string line, out string store, out string owner); }
