using System.Globalization;

namespace Hw8.Exercise0.Core;

public static class ValidateDate
{
    public static bool IsValidDate(string date)
    {
        var todayDate = DateTime.Today.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        return date.Equals(todayDate, StringComparison.OrdinalIgnoreCase);
    }
}
