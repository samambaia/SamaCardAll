using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.VO;
using System.Net;
using Microsoft.Extensions.Logging;
using SamaCardAll.Core.Interfaces;

namespace SamaCardAll.Core.Services
{
    public class ReportService : IReportService
    {
        public ReportService(AppDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetDistinctInstallmentMonthYear()
        {
            var monthYears = await _context.Installments
                                 .Select(i => i.MonthYear)
                                 .Distinct()
                                 .ToListAsync();

            return monthYears.OrderBy(my => ConvertMonthYearToInt(my));
        }

        private static int ConvertMonthYearToInt(string monthYear)
        {
            var parts = monthYear.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]);
            return year * 100 + month;
        }

        /*
            * The method GetFilteredInstallments is used to retrieve the installments for a specific customer and month/year.
            * The method receives the customer ID and the month/year as parameters.
            * The month/year parameter is URL encoded, so it is decoded before being used.
            * The method retrieves the installments from the database using the GetInstallments method.
            * The GetInstallments method returns an IQueryable<Installments> object that includes the related Spend, Customer, and Card entities.
            * The method then maps the installments to a list of InvoiceDto objects using the MapToInvoiceDto method.
            * Finally, the method returns the list of InvoiceDto objects.
        */
        public async Task<IEnumerable<InvoiceVO>> GetFilteredInstallments(int? customerId, string monthYear)
        {
            monthYear = WebUtility.UrlDecode(monthYear);

            var installments = GetInstallments(customerId, monthYear);

            var results = MapToInvoiceDto(installments);

            return await Task.FromResult(results);
        }

        // Do not use! The Installment field was updated on the Installment table
        public async Task UpdateInstallments()
        {
            var spends = await _context.Spends.ToListAsync();
            foreach (var spend in spends)
            {
                // Retrieve installments for the current spend
                var installments = await _context.Installments
                                                .Where(i => i.SpendIdSpend == spend.IdSpend)
                                                .ToListAsync();

                // Logic to generate installment numbers (e.g., "01/05")
                int counter = 1;
                foreach (var installment in installments)
                {
                    installment.Installment = counter.ToString("00") + "/" + spend.InstallmentPlan.ToString("00");
                    counter++;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<InvoiceVO>> GetTotalCustomerPerMonth(string monthYear)
        {
            string decodedMonthYear = WebUtility.UrlDecode(monthYear);

            var query = _context.Installments
                                .Include(i => i.Spend.Customer)
                                .Where(i => i.MonthYear == decodedMonthYear && i.Spend.Deleted == 0)
                                .GroupBy(i => new { i.Spend.Customer.CustomerName, i.MonthYear })
                                .Select(g => new
                                {
                                    g.Key.MonthYear,
                                    g.Key.CustomerName,
                                    InstallmentValues = g.Select(i => i.InstallmentValue)
                                });

            var result = await query.ToListAsync(); // Force execution on the client asynchronously

            return result.Select(g => new InvoiceDto
            {
                MonthYear = g.MonthYear,
                CustomerName = g.CustomerName,
                InstallmentAmount = g.InstallmentValues.Sum()
            });
        }

        public async Task<IEnumerable<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear)
        {
            string decodedMonthYear = WebUtility.UrlDecode(monthYear);
            var query = _context.Installments
                .Include(c => c.Spend.Card)
                .Where(c => c.MonthYear == decodedMonthYear && c.Spend.Deleted == 0)
                .GroupBy(c => new { c.Spend.Card.IdCard, c.Spend.Card.Bank, c.MonthYear })
                .Select(d => new
                {
                    d.Key.MonthYear,
                    d.Key.IdCard,
                    d.Key.Bank,
                    InstallmentTotal = d.Select(c => c.InstallmentValue)
                });

            var result = await query.ToListAsync();

            return result.Select(g => new TotalCardMonthYearDTO
            {
                MonthYear = g.MonthYear,
                CardName = g.Bank,
                InstallmentAmount = g.InstallmentTotal.Sum(),
                IdCard = g.IdCard
            });
        }

        public async Task<List<Decimal>> SummarizeSpends(string monthYear)
        {
            try
            {
                _logger.LogInformation("SummarizeSpends called with monthYear: {monthYear}", monthYear);
                string decodedMonthYear = WebUtility.UrlDecode(monthYear);
                _logger.LogInformation("Decoded monthYear: {decodedMonthYear}", decodedMonthYear);

                var totalSpends = await _context.Installments
                                                .Include(i => i.Spend)
                                                .Where(i => i.MonthYear == decodedMonthYear && i.Spend != null && i.Spend.Deleted == 0)
                                                .Select(i => i.InstallmentValue)
                                                .ToListAsync();

                _logger.LogInformation("Total spends: {totalSpends}", totalSpends);
                return totalSpends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in SummarizeSpends for {monthYear}", monthYear);
                throw;
            }
        }

        public Task<List<CardVO>> DetailedCard(int? cardId, string? monthYear)
        {
            try
            {
                _logger.LogInformation("List spends by Card and MonthYear: {cardId} - {monthYear}", cardId, monthYear);
                string decodedMonthYear = WebUtility.UrlDecode(monthYear);
                _logger.LogInformation("Decoded monthYear: {decodedMonthYear}", decodedMonthYear);

                var query = _context.Installments
                                    .Include(i => i.Spend)
                                    .Include(i => i.Spend.Customer)
                                    .Include(i => i.Spend.Card)
                                    .Where(i => i.Spend.Card.IdCard == cardId && i.MonthYear == decodedMonthYear && i.Spend.Deleted == 0)
                                    .Select(i => new CardVO
                                    {
                                        IdCard = i.Spend.Card.IdCard,
                                        CardName = i.Spend.Card.Bank,
                                        DescriptionSpend = i.Spend.Expenses,
                                        CustomerName = i.Spend.Customer.CustomerName,
                                        InstallmentAmount = i.InstallmentValue,
                                        MonthYear = i.MonthYear,
                                        Installment = i.Installment
                                    });

                return query.ToListAsync();
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred in DetailedCard for {cardId} - {monthYear}", cardId, monthYear);
                throw;
            }
        }
    }
}
