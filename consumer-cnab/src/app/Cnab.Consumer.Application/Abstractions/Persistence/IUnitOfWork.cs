namespace Cnab.Consumer.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
}
