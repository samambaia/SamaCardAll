using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Models;
using System.Net;

namespace SamaCardAll.Infra.Repository
{
    public class ReportRepository(AppDbContext context) : IReportRepository
    {
        public Task<List<DetailedCardVO>> DetailedCard(int? cardId, string monthYear)
        {
            try
            {
                string decodedMonthYear = WebUtility.UrlDecode(monthYear);

                var query = context.Installments
                                    .Where(i => (!cardId.HasValue || i.Spend.Card.IdCard == cardId) &&
                                                i.MonthYear == decodedMonthYear &&
                                                i.Spend.Deleted == 0)
                                    .Select(i => new DetailedCardVO
                                    (
                                        i.Spend.Card.IdCard,
                                        i.Spend.Card.Bank,
                                        i.Spend.Expenses,
                                        i.Spend.Customer.CustomerName,
                                        i.InstallmentValue,
                                        i.MonthYear,
                                        i.Installment.ToString()
                                    ));

                return query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>> GetDistinctInstallmentMonthYear()
        {
            var monthYears = await context.Installments
                                 .Select(i => i.MonthYear)
                                 .Distinct()
                                 .ToListAsync();

            return [.. monthYears.OrderBy(my => ConvertMonthYearToInt(my))];
        }

        public async Task<List<InvoiceVO>> GetFilteredInstallments(int? customerId, string monthYear)
        {
            monthYear = WebUtility.UrlDecode(monthYear);

            var installments = GetInstallments(customerId, monthYear);

            var results = MapToInvoiceDto(installments);

            return await Task.FromResult(results);
        }

        public Task<List<TotalCardMonthYearDTO>> GetTotalCardMonthYear(string monthYear)
        {
            throw new NotImplementedException();
        }

        public Task<List<InvoiceVO>> GetTotalCustomerPerMonth(string monthYear)
        {
            throw new NotImplementedException();
        }

        public Task<List<decimal>> SummarizeSpends(string monthYear)
        {
            throw new NotImplementedException();
        }

        public Task UpdateInstallments()
        {
            throw new NotImplementedException();
        }

        /*
         * Privates methods
         */

        private static int ConvertMonthYearToInt(string monthYear)
        {
            var parts = monthYear.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]);
            return year * 100 + month;
        }

        private IQueryable<Installments> GetInstallments(int? customerId, string monthYear)
        {
            var result = context.Installments
                            .Include(i => i.Spend)
                            .Include(i => i.Spend.Customer)
                            .Include(i => i.Spend.Card)
                            .Where(i => i.MonthYear == monthYear && i.Spend.Customer.IdCustomer == customerId && i.Spend.Deleted == 0);

            return result;
        }

        private static List<InvoiceVO> MapToInvoiceVO(IQueryable<Installments> installments)
        {
            return installments.Select(i => new InvoiceVO
            (
                i.Spend.CardIdCard,
                i.MonthYear,
                i.,

                i.Spend.Expenses,
                i.Spend.Customer.CustomerName,
                i.Spend.Card.Bank,
                i.InstallmentValue,
                i.MonthYear,
                i.Installment
            )).ToList();
        }

    }
}
