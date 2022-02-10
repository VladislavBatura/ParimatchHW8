using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public static class MatchedCurrencies
{
    public static IList<Currency> Get(IEnumerable<Currency> listCurrency, OperationData data)
    {
        return listCurrency.Where(x => x.CurrencyCode
                .Equals(data.OriginalCurrency, StringComparison.OrdinalIgnoreCase)
            || x.CurrencyCode
                .Equals(data.DestinationCurrency, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
