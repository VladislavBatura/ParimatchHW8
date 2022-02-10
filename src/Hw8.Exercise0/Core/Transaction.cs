using System.Globalization;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public static class Transaction
{
    //Suppressed this message because it's highlight "else-if" statement for "simplifing" if-statement,
    //but if i do that, it will become less readable
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public static ResultTransaction ProcessTransaction(IEnumerable<Currency> listCurrency, OperationData data)
    {
        var result = MatchedCurrencies.Get(listCurrency, data);
        var firstRate = -1m;
        var secondRate = -1m;

        if (result.Count > 0)
        {
            firstRate = result[0].Rate;
            if (result.Count == 2)
                secondRate = result[1].Rate;
        }

        if (data.OriginalCurrency.Equals(data.DestinationCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(data.OriginalCurrency, data.Amount,
                DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo));
        }
        else if (data.DestinationCurrency.Equals(DefaultCurrency.Uah, StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(DefaultCurrency.Uah,
                AmountToRatesUAH(firstRate, data.Amount),
                result.First().ExchangeDate);
        }
        else if (data.OriginalCurrency.Equals(DefaultCurrency.Uah, StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(result.First().CurrencyCode,
                AmountToRatesUAH(firstRate, data.Amount),
                result.First().ExchangeDate);
        }
        else
        {
            return new ResultTransaction(result[1].CurrencyCode,
               AmountToRates(firstRate, data.Amount, secondRate),
               result.First().ExchangeDate);
        }
    }

    private static decimal AmountToRatesUAH(decimal rate, decimal amount)
    {
        return rate * amount;
    }
    private static decimal AmountToRates(decimal rate, decimal amount, decimal secondRate)
    {
        return AmountToRatesUAH(secondRate, AmountToRatesUAH(rate, amount));
    }
}
