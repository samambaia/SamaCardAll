using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface IReportService
    {
        Task<List<InvoiceVO>> GetFilteredInstallments(int? customerId, string monthYear);
        Task UpdateInstallments();
        Task<List<string>> GetDistinctInstallmentMonthYear();
        Task<List<InvoiceVO>> GetTotalCustomerPerMonth(string monthYear);
        Task<List<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear);
        Task<List<decimal>> SummarizeSpends(string monthYear);
        Task<List<CardVO>> DetailedCard(int? cardId, string monthYear);
    }
}
