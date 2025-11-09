namespace Cnab.Consumer.Domain.Entities;

public class Transaction
{
    public int Id { get; private set; }
    public int Type { get; private set; }
    public string Nature { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public decimal SignedValue { get; private set; }
    public string Cpf { get; private set; } = string.Empty;
    public string Card { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }
    public int StoreId { get; private set; }
    public Store? Store { get; private set; }

    public Transaction(int type, string nature, decimal value, decimal signedValue, string cpf, string card, DateTime occurredAt)
    {
        Type = type;
        Nature = nature;
        Value = value;
        SignedValue = signedValue;
        Cpf = cpf;
        Card = card;
        OccurredAt = occurredAt;
    }    

    public void SetIdStore(int storeId)
    {
        this.StoreId = storeId;
    }

    public void SetStore(Store store)
    {
        this.Store = store;
    }
}
