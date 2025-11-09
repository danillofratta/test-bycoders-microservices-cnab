using Cnab.Api.Domain.Entities;
using System;
using System.Globalization;

namespace Cnab.Api.Domain.Services;

public class CnabParser : ICnabParser
{
    private static string Slice(string s, int start, int endIncl)
    {
        int i = start - 1;
        return s.Substring(i, endIncl - start + 1);
    }

    public Transaction Parse(string line, out string storeName, out string storeOwner)
    {
        var type = int.Parse(Slice(line, 1, 1));

        var date = Slice(line, 2, 9);
        var cents = Slice(line, 10, 19);

        var cpf = Slice(line, 20, 30).Trim();
        var card = Slice(line, 31, 42).Trim();

        var time = Slice(line, 43, 48);
        storeOwner = Slice(line, 49, 62).Trim();
        storeName = Slice(line, 63, 81).Trim();

        var val = decimal.Parse(cents, CultureInfo.InvariantCulture) / 100m;

        var dt = DateTime.ParseExact(date + time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        var tz = TimeZoneInfo.CreateCustomTimeZone("UTC-3", TimeSpan.FromHours(-3), "UTC-3", "UTC-3");
        var when = TimeZoneInfo.ConvertTimeToUtc(dt, tz);

        var (nature, sign) = type switch
        {
            1 => ("Income", +1),
            2 => ("Expense", -1),
            3 => ("Expense", -1),
            4 => ("Income", +1),
            5 => ("Income", +1),
            6 => ("Income", +1),
            7 => ("Income", +1),
            8 => ("Income", +1),
            9 => ("Expense", -1),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Transaction(type, nature, val, sign * val, cpf, card, when);
    }
}
