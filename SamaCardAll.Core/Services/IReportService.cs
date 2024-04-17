using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.DTO;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public interface IReportService
    {
        Task<IEnumerable<InvoiceDto>> GetFilteredInstallments(int? customerId, string? monthYear);
    }
}
