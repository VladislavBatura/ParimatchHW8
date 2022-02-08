namespace Hw8.Exercise0.Models;

public class OperationData
{
    public string OriginalCurrency { get; }
    public string DestinationCurrency { get; }
    public decimal Amount { get; }

    public OperationData(string originalCurrency, string destinationCurrency, decimal amount)
    {
        OriginalCurrency = originalCurrency;
        DestinationCurrency = destinationCurrency;
        Amount = amount;
    }
}
