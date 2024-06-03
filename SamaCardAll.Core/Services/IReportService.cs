using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.DTO;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public interface IReportService
    {
        Task<IEnumerable<InvoiceDto>> GetFilteredInstallments(int? customerId, string? monthYear);
        Task UpdateInstallments();
        Task<IEnumerable<string>> GetDistinctInstallmentMonthYear();
        Task<IEnumerable<InvoiceDto>> GetTotalCustomerPerMonth(string monthYear);
        Task<IEnumerable<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear);
        Task<decimal> SummarizeSpends(string monthYear);
    }
}
