using System.Text.Json.Serialization;

namespace Hw8.Exercise0.Models;

public class Currency
{
    [JsonPropertyName("cc")]
    public string CurrencyCode { get; }
    [JsonPropertyName("txt")]
    public string CurrencyName { get; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; }
    [JsonPropertyName("exchangedate")]
    public string ExchangeDate { get; }

    public Currency(string currencyCode, string currencyName, decimal rate, string exchangeDate)
    {
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
        Rate = rate;
        ExchangeDate = exchangeDate;
    }
}
