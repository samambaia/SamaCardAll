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

        public async Task<IEnumerable<string>> GetDistinctInstallmentMonthYear()
        {
            var monthYears = await _context.Installments
                                 .Select(i => i.MonthYear)
                                 .Distinct()
                                 .ToListAsync();

            return monthYears.OrderBy(my => ConvertMonthYearToInt(my));

            //var monthYearData = monthYears.Select(my => new MonthYearData
            //{
            //    SortValue = ConvertMonthYearToInt(my),
            //    OriginalMonthYear = my
            //})
            //.ToArray();

            //Array.Sort(monthYearData, (x, y) => x.SortValue.CompareTo(y.SortValue));

            //return (Task<IEnumerable<string>>)monthYearData.Select(data => data.OriginalMonthYear);
        }

        //struct MonthYearData
        //{
        //    public int SortValue { get; set; }
        //    public string OriginalMonthYear { get; set; }
        //}

        static int ConvertMonthYearToInt(string monthYear)
        {
            var parts = monthYear.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]);
            return year * 100 + month;
        }

        public async Task<IEnumerable<InvoiceDto>> GetFilteredInstallments(int? customerId, string? monthYear)
        {
            var query = _context.Installments
                                .Include(i => i.Spend)
                                .Include(i => i.Spend.Customer)
                                .Include(i => i.Spend.Card)
                                .Where(i => i.MonthYear == WebUtility.UrlDecode(monthYear) && i.Spend.Customer.IdCustomer == customerId)
                                .Where(i => i.Spend.Deleted == 0); //Filtered by Spends that doesn't have a Deleted flag

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

        // Do not use! The Installment field was updated in the Installment table
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

        public async Task<IEnumerable<InvoiceDto>> GetTotalCustomerPerMonth(string monthYear)
        {
            string decodedMonthYear = WebUtility.UrlDecode(monthYear);

            var query = _context.Installments
                                .Include(i => i.Spend.Customer)
                                .Where(i => i.MonthYear == decodedMonthYear)
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
            string decodedMonthYear = WebUtility.UrlDecode (monthYear);
            var query = _context.Installments
                .Include(c => c.Spend.Card)
                .Where(c => c.MonthYear == decodedMonthYear)
                .GroupBy(c => new { c.Spend.Card.Bank, c.MonthYear })
                .Select(d => new
                {
                    d.Key.MonthYear,
                    d.Key.Bank,
                    InstallmentTotal = d.Select(c => c.InstallmentValue)
                });

            var result = await query.ToListAsync();

            return result.Select(g => new TotalCardMonthYearDTO
            {
                MonthYear = g.MonthYear,
                CardName = g.Bank,
                InstallmentAmount = g.InstallmentTotal.Sum()
            });
        }
    }
}
