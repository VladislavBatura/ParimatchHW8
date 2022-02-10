using System.Globalization;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public static class DefaultCurrency
{
    public const string Uah = "uah";
    public static readonly Currency Hrivna = new("UAH",
            "Ukrainian hrivna",
            1m,
            DateTime.Today.ToString(DateTimeFormatInfo.InvariantInfo));
}
