using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.DTO;
using SamaCardAll.Infra;
using System.Net;

namespace SamaCardAll.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDto>> GetFilteredInstallments(int? customerId, string? monthYear)
        {
            var query = _context.Installments
                                .Include(i => i.Spend)
                                .Include(i => i.Spend.Customer)
                                .Include(i => i.Spend.Card)
                                .Where(i => i.MonthYear == WebUtility.UrlDecode(monthYear) && i.Spend.Customer.IdCustomer == customerId);

            // Projection (Map to DTO)
            var results = await query.Select(i => new InvoiceDto
            {
                DescriptionSpend = i.Spend.Expenses,
                CustomerName = i.Spend.Customer.CustomerName,
                CardName = i.Spend.Card.Bank,
                InstallmentAmount = i.InstallmentValue,
                MonthYear = i.MonthYear,
                Installment = i.Installment

            }).ToListAsync();

            return results;
        }

        public async Task UpdateInstallments()
        {
            var spends = await _context.Spends.ToListAsync();
            foreach (var spend in spends)
            {
                // Retrieve installments for the current spend
                var installments = await _context.Installments
                                                .Where(i => i.SpendIdSpend == spend.IdSpend)
                                                .ToListAsync();

                // Your logic to generate installment numbers (e.g., "01/05")
                int counter = 1;
                foreach (var installment in installments)
                {
                    installment.Installment = counter.ToString("00") + "/" + spend.InstallmentPlan.ToString("00");
                    counter++;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
