namespace Cnab.Api.Domain.Entities;

public class Store
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Owner { get; private set; } = string.Empty;

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public Store(string name, string owner)
    {
        Name = name.Trim();
        Owner = owner.Trim();
    }
}
