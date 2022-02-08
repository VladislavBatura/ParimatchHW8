using ISO._4217;

namespace Hw8.Exercise0.Core;

public static class ArgumentsHandler
{
    public static bool IsValidArgs(string[] args)
    {
        if (args == null || args.Length != 3)
        {
            return false;
        }

        if (!decimal.TryParse(args[2], out _))
            return false;

        for (var i = 0; i < args.Length - 1; i++)
        {
            var returnedCurrency = CurrencyCodesResolver.GetCurrenciesByCode(args[i]);
            if (args[i].Length != 3)
            {
                return false;
            }
            else if (!returnedCurrency.Any() ||
                    returnedCurrency.First().Code.Equals("xxx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }
}
