using System.Net;

public static class MonthYearExtensions
{
    public static string DecodeMonthYear(this string monthYear)
    {
        return WebUtility.UrlDecode(monthYear);
    }

    public static int ToMonthYearInt(this string monthYear)
    {
        var parts = monthYear.Split('/');
        int month = int.Parse(parts[0]);
        int year = int.Parse(parts[1]);
        return year * 100 + month;
    }
}
