using SamaCardAll.Shared.Contracts.Report;

namespace SamaCardAll.Core.Interfaces
{
    public interface IReportService
    {
        Task<List<InvoiceDTO>> GetFilteredInstallments(int? customerId, string monthYear);
        Task UpdateInstallments();
        Task<List<string>> GetDistinctInstallmentMonthYear();
        Task<List<InvoiceDTO>> GetTotalCustomerPerMonth(string monthYear);
        Task<List<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear);
        Task<List<decimal>> SummarizeSpends(string monthYear);
        Task<List<DetailedCardDTO>> DetailedCard(int? cardId, string monthYear);
    }
}
