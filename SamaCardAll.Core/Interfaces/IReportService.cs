using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.DTO;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<InvoiceDto>> GetFilteredInstallments(int? customerId, string? monthYear);
        Task UpdateInstallments();
        Task<IEnumerable<string>> GetDistinctInstallmentMonthYear();
        Task<IEnumerable<InvoiceDto>> GetTotalCustomerPerMonth(string monthYear);
        Task<IEnumerable<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear);
        Task<List<decimal>> SummarizeSpends(string monthYear);
        Task<List<DetailedCardDTO>> DetailedCard(int? cardId, string? monthYear);
    }
}
