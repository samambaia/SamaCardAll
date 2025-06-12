using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository repository)
        {
            _reportRepository = repository;
        }

        public async Task<List<string>> GetDistinctInstallmentMonthYear()
        {
            var monthYears = await _reportRepository.GetDistinctInstallmentMonthYear();
            return monthYears.OrderBy(my => my.ToMonthYearInt()).ToList();
        }

        public async Task<List<InvoiceVO>> GetFilteredInstallments(int? customerId, string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var installments = await _reportRepository.GetFilteredInstallments(customerId, decodedMonthYear);
            return [.. installments.OrderBy(i => i.Installment).ThenBy(i => i.Installment)];
        }

        public async Task<List<InvoiceVO>> GetTotalCustomerPerMonth(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var invoices = await _reportRepository.GetTotalCustomerPerMonth(decodedMonthYear);
            return [.. invoices.OrderBy(i => i.CustomerName).ThenBy(i => i.MonthYear)];
        }

        public async Task<List<TotalCardMonthYearVO>> GetTotalCardMonthYear(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var totalCards = await _reportRepository.GetTotalCardMonthYear(decodedMonthYear);
            return [.. totalCards.OrderBy(tc => tc.CardName).ThenBy(tc => tc.MonthYear)];
        }

        public async Task<List<Decimal>> SummarizeSpends(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var spends = await _reportRepository.SummarizeSpends(decodedMonthYear);
            return [.. spends.OrderBy(s => s)];
        }

        public async Task<List<DetailedCardVO>> DetailedCard(int? cardId, string? monthYear)
        {
            string decodedMonthYear = monthYear?.DecodeMonthYear() ?? string.Empty;

            return await _reportRepository.DetailedCard(cardId, decodedMonthYear);
        }

        public Task UpdateInstallments()
        {
            throw new NotImplementedException();
        }

        // Do not use! The Installment field was updated on the Installment table
        //public async Task UpdateInstallments()
        //{
        //    var spends = await _context.Spends.ToListAsync();
        //    foreach (var spend in spends)
        //    {
        //        // Retrieve installments for the current spend
        //        var installments = await _context.Installments
        //                                        .Where(i => i.SpendIdSpend == spend.IdSpend)
        //                                        .ToListAsync();

        //        // Logic to generate installment numbers (e.g., "01/05")
        //        int counter = 1;
        //        foreach (var installment in installments)
        //        {
        //            installment.Installment = counter.ToString("00") + "/" + spend.InstallmentPlan.ToString("00");
        //            counter++;
        //        }

        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
