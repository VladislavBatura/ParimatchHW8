using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public static class TransactionArgumentsHandler
{
    public static OperationData ParseArgs(string[] args)
    {
        var amount = -1m;
        if (decimal.TryParse(args[2], out var res))
            amount = res;
        return new OperationData(args[0], args[1], amount);
    }

    public static bool IsValidArgs(OperationData data, IEnumerable<Currency> listCurrency)
    {
        var result = MatchedCurrencies.Get(listCurrency, data);

        if (data.OriginalCurrency.Equals(DefaultCurrency.Uah, StringComparison.OrdinalIgnoreCase))
        {
            result.Insert(0, DefaultCurrency.Hrivna);
        }
        if (data.DestinationCurrency.Equals(DefaultCurrency.Uah, StringComparison.OrdinalIgnoreCase))
        {
            result.Add(DefaultCurrency.Hrivna);
        }

        return result.Count == 2 && data.Amount != -1m;
    }
}
