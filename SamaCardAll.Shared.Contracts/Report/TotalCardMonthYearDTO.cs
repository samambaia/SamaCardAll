namespace SamaCardAll.Shared.Contracts.Report
{
    public record TotalCardMonthYearDTO(int IdCard, string CardName, decimal InstallmentAmount, string MonthYear) : IEquatable<TotalCardMonthYearDTO>;
}
