namespace Hw8.Exercise0.Models;

public class ResultTransaction
{
    public string Currency { get; }
    public decimal Amount { get; }
    public string Date { get; }

    public ResultTransaction(string currency, decimal amount, string date)
    {
        Currency = currency;
        Amount = amount;
        Date = date;
    }
}
