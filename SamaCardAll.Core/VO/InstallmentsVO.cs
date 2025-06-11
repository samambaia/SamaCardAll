namespace SamaCardAll.Core.VO
{
    public record InstallmentsVO(
        int IdInstallment,
        int IdCard,
        string MonthYear,
        decimal InstallmentAmount,
        string InstallmentStatus
    );
}
