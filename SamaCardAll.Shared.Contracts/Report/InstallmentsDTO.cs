namespace SamaCardAll.Shared.Contracts.Report
{
    public record InstallmentsDTO(
        int IdInstallment,
        int IdCard,
        string MonthYear,
        decimal InstallmentAmount,
        string InstallmentStatus
    );
}
