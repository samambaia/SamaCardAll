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
            string decodedMonthYear = monthYear.DecodeMonthYear();

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

        public async Task<List<string>> GetDistinctInstallmentMonthYear()
        {
            var monthYears = await context.Installments
                                 .Select(i => i.MonthYear)
                                 .Distinct()
                                 .ToListAsync();
            return [.. monthYears.OrderBy(my => my.ToMonthYearInt())];
        }

        /*
        * The method GetFilteredInstallments is used to retrieve the installments for a specific customer and month/year.
        * The method receives the customer ID and the month/year as parameters.
        * The month/year parameter is URL encoded, so it is decoded before being used.
        * The method retrieves the installments from the database using the GetInstallments method.
        * The GetInstallments method returns an IQueryable<Installments> object that includes the related Spend, Customer, and Card entities.
        * The method then maps the installments to a list of InvoiceVO objects using the MapToInvoiceVO method.
        * Finally, the method returns the list of InvoiceDto objects.
        */
        public async Task<List<InvoiceVO>> GetFilteredInstallments(int? customerId, string monthYear)
        {
            string decodeMonthYear = monthYear.DecodeMonthYear();

            var installments = GetInstallments(customerId, decodeMonthYear);
            
            var results = MapToInvoiceVO(installments);

            return await Task.FromResult(results);
        }

        public async Task<List<TotalCardMonthYearVO>> GetTotalCardMonthYear(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var query = await context.Installments
                .Include(c => c.Spend.Card)
                .Where(c => c.MonthYear == decodedMonthYear && c.Spend.Deleted == 0)
                .GroupBy(c => new { c.Spend.Card.IdCard, c.Spend.Card.Bank, c.MonthYear })
                .Select(d => new
                {
                    d.Key.MonthYear,
                    d.Key.IdCard,
                    d.Key.Bank,
                    InstallmentTotal = d.Select(c => c.InstallmentValue)
                }).ToListAsync();

            var result = query.Select(g => new TotalCardMonthYearVO
            (
                IdCard: g.IdCard,
                CardName: g.Bank,
                InstallmentAmount: g.InstallmentTotal.Sum(),
                MonthYear: g.MonthYear
            )).ToList();

            return result;
        }

        public async Task<List<InvoiceVO>> GetTotalCustomerPerMonth(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var query = await context.Installments
                               .Include(i => i.Spend.Customer)
                               .Where(i => i.MonthYear == decodedMonthYear && i.Spend.Deleted == 0)
                               .GroupBy(i => new { i.Spend.Customer.CustomerName, i.MonthYear })
                               .Select(g => new
                               {
                                   g.Key.MonthYear,
                                   g.Key.CustomerName,
                                   InstallmentValues = g.Select(i => i.InstallmentValue)
                               }).ToListAsync();

            var result = query.Select(q => new InvoiceVO
            (
                descriptionSpend: string.Empty,
                customerName: q.CustomerName,
                cardName: string.Empty,
                installmentAmount: q.InstallmentValues.Sum(),
                monthYear: q.MonthYear,
                installment: string.Empty
            )).ToList();

            return result;
        }

        public async Task<List<decimal>> SummarizeSpends(string monthYear)
        {
            string decodedMonthYear = monthYear.DecodeMonthYear();

            var totalSpends = await context.Installments
                                            .Include(i => i.Spend)
                                            .Where(i => i.MonthYear == decodedMonthYear && i.Spend != null && i.Spend.Deleted == 0)
                                            .Select(i => i.InstallmentValue)
                                            .ToListAsync();

            return totalSpends;
        }

        public Task UpdateInstallments()
        {
            throw new NotImplementedException();
        }

        /*
         * Privates methods
         */

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
                i.Spend.Expenses,
                i.Spend.Customer.CustomerName,
                i.Spend.Card.Bank,
                i.InstallmentValue,
                i.MonthYear,
                i.Installment.ToString()
            )).ToList();
        }

    }
}
