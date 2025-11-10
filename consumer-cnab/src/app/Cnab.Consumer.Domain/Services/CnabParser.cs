using System.Globalization;
using Cnab.Consumer.Domain.Entities;

namespace Cnab.Consumer.Domain.Services;

public interface ICnabParser
{
    Transaction Parse(string line, out string store, out string owner);
}
public class CnabParser : ICnabParser
{
    private static string SliceSafe(string s, int start, int endIncl)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        int i = start - 1;
        if (i < 0) i = 0;

        // minimum lenght
        if (s.Length < endIncl)
            s = s.PadRight(endIncl, ' ');

        int len = Math.Min(s.Length - i, endIncl - start + 1);
        return s.Substring(i, len);
    }

    public Transaction Parse(string rawLine, out string storeName, out string storeOwner)
    {
        // ensures padding (each row must have 81 positions)
        var line = rawLine.Length < 81 ? rawLine.PadRight(81, ' ') : rawLine;

        var type = int.Parse(SliceSafe(line, 1, 1));
        var date = SliceSafe(line, 2, 9);
        var cents = SliceSafe(line, 10, 19);
        var cpf = SliceSafe(line, 20, 30).Trim();
        var card = SliceSafe(line, 31, 42).Trim();
        var time = SliceSafe(line, 43, 48);
        storeOwner = SliceSafe(line, 49, 62).Trim();
        storeName = SliceSafe(line, 63, 81).Trim();

        var val = decimal.Parse(cents, CultureInfo.InvariantCulture) / 100m;
        var dt = DateTime.ParseExact(date + time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        // UTC-3 layout CNAB ByCoders
        var when = new DateTimeOffset(dt, TimeSpan.FromHours(-3)).UtcDateTime;

        // signal
        var (nature, sign) = type switch
        {
            1 => ("Debit", +1),
            2 => ("Boleto", -1),
            3 => ("Financing", -1),
            4 => ("Credit", +1),
            5 => ("LoanReceipt", +1),
            6 => ("Sales", +1),
            7 => ("TEDReceipt", +1),
            8 => ("DOCReceipt", +1),
            9 => ("Rent", -1),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Tipo de transação inválido: {type}")
        };

        return Transaction.Create(type, nature, val, sign * val, cpf, card, when, storeName, storeOwner);
    }
}
