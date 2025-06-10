using SamaCardAll.Core.DTO;

namespace SamaCardAll.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<List<InvoiceDto>> GetFilteredInstallments(int? customerId, string monthYear);
        Task UpdateInstallments();
        Task<List<string>> GetDistinctInstallmentMonthYear();
        Task<List<InvoiceDto>> GetTotalCustomerPerMonth(string monthYear);
        Task<List<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear);
        Task<List<decimal>> SummarizeSpends(string monthYear);
        Task<List<CardVO>> DetailedCard(int? cardId, string monthYear);
    }
}
