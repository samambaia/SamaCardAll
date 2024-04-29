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
            var monthYears = _context.Installments
                                 .Select(i => i.MonthYear)
                                 .Distinct()
                                 .ToList();


            var monthYearData = monthYears.Select(my => new MonthYearData
            {
                SortValue = ConvertMonthYearToInt(my),
                OriginalMonthYear = my
            })
            .ToArray();

            Array.Sort(monthYearData, (x, y) => x.SortValue.CompareTo(y.SortValue));

            return monthYearData.Select(data => data.OriginalMonthYear);

            // Now you can access both the sorted order and original MonthYear values:
            //foreach (var data in monthYearData)
            //{
            //    Console.WriteLine($"SortValue: {data.SortValue}  Original MonthYear: {data.OriginalMonthYear}");
            //}

        }

        struct MonthYearData
        {
            public int SortValue { get; set; }
            public string OriginalMonthYear { get; set; }
        }

        private int ConvertMonthYearToInt(string monthYear)
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
