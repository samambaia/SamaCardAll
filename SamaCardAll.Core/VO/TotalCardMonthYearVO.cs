namespace SamaCardAll.Core.VO
{
    public record TotalCardMonthYearVO(int IdCard, string CardName, decimal InstallmentAmount, string MonthYear) : IEquatable<TotalCardMonthYearVO>;
}
