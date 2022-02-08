using System.Globalization;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public static class Transaction
{
    private const string Uah = "uah";

    private static readonly Currency _hrivna = new("UAH",
                "Ukrainian hrivna",
                1m,
                DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo));

    public static OperationData ParseArgs(string[] args)
    {
        var amount = -1m;
        if (decimal.TryParse(args[2], out var res))
            amount = res;
        return new OperationData(args[0], args[1], amount);
    }

    public static bool IsValidArgs(OperationData data, IEnumerable<Currency> listCurrency)
    {
        var result = GetMatchedCurrencies(listCurrency, data);

        if (data.OriginalCurrency.Equals(Uah, StringComparison.OrdinalIgnoreCase))
        {
            result.Insert(0, _hrivna);
        }
        if (data.DestinationCurrency.Equals(Uah, StringComparison.OrdinalIgnoreCase))
        {
            result.Add(_hrivna);
        }

        return result.Count == 2 && data.Amount != -1m;
    }

    //Suppressed this message because it's highlight 66 line for "simplifing" if-statement,
    //but if i do that, it will become less readable
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public static ResultTransaction ProcessTransaction(IEnumerable<Currency> listCurrency, OperationData data)
    {
        var result = GetMatchedCurrencies(listCurrency, data);
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
        else if (data.DestinationCurrency.Equals(Uah, StringComparison.OrdinalIgnoreCase))
        {
            return new ResultTransaction(Uah,
                AmountToRatesUAH(firstRate, data.Amount),
                result.First().ExchangeDate);
        }
        else if (data.OriginalCurrency.Equals(Uah, StringComparison.OrdinalIgnoreCase))
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

    private static IList<Currency> GetMatchedCurrencies(IEnumerable<Currency> listCurrency, OperationData data)
    {
        return listCurrency.Where(x => x.CurrencyCode
                .Equals(data.OriginalCurrency, StringComparison.OrdinalIgnoreCase)
            || x.CurrencyCode
                .Equals(data.DestinationCurrency, StringComparison.OrdinalIgnoreCase))
            .ToList();
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
