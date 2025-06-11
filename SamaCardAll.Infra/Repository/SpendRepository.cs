using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Mapping;
using SamaCardAll.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Infra.Repository
{
    public class SpendRepository : ISpendRepository
    {
        private readonly AppDbContext _context;
        private readonly List<Installments> installmentsExist;
        private readonly List<Installments> installments = new();

        public SpendRepository(AppDbContext context)
        {
            _context = context;

            var q = _context.Installments
                .Include(s => s.Spend);

            installmentsExist = q.ToList();
        }

        public async Task CreateAsync(SpendVO spend)
        {
            // Define de ID
            int newIdSpend = await _context.Spends.AnyAsync() ? await _context.Spends.MaxAsync(s => s.IdSpend) + 1 : 1;

            // Generate the Installment List
            var installmentList = GenerateInstallmentPlan(newIdSpend, spend.InstallmentPlan, spend.InstallmentValue, spend.Date);

            // Add a new expense
            _context.Add(spend);
            // Add Installment List
            _context.Installments.AddRange(installmentList);

            _context.SaveChanges();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var spend = await _context.Spends.FindAsync(id);
            if (spend is null) return false;

            spend.Deleted = 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SpendVO> GetByIdAsync(int id)
        {
            var getSpend = await _context.Spends.FindAsync(id);
            return getSpend.ToVO();
        }

        public async Task<List<SpendVO>> GetSpendsAsync()
        {
            var getSpends = await _context.Spends.ToListAsync();

            return [.. getSpends.Select(s => s.ToVO())];

        }

        public async Task<bool> UpdateAsync(SpendVO spend)
        {
            var getSpend = await _context.Spends.FindAsync(spend.IdSpend);

            if (getSpend == null)
            {
                return false;
            }

            getSpend.Expenses = spend.Expenses;
            getSpend.Amount = spend.Amount;
            getSpend.Date = spend.Date;
            getSpend.InstallmentPlan = spend.InstallmentPlan;
            getSpend.InstallmentValue = spend.InstallmentValue;
            getSpend.Deleted = spend.Deleted;
            getSpend.CustomerIdCustomer = spend.CustomerIdCustomer;
            getSpend.CardIdCard = spend.CardIdCard;
            getSpend.UserIdUser = spend.UserIdUser;
            getSpend.CreatedDate = spend.CreatedDate;

            _context.Spends.Update(getSpend);
            await _context.SaveChangesAsync();
            return true;
        }

        /*
         * 
         * Private Methods
         * 
         */

        private List<Installments> GenerateInstallmentPlan(int spendId, int installmentPlan, decimal installmentValue, DateTime purchaseDate)
        {
            string installmentDisplay = string.Empty;

            int maxId = 1;
            // Calculate Installment ID
            if (installmentsExist.Count > 0)
            {
                maxId = installmentsExist.Max(s => s.Id) + 1;
            }

            // Calculate the interval of months between each installment
            int monthsInterval = 1;

            decimal amount = 0;
            DateTime dueDate;

            // Add installments to the intallment plan
            for (int i = 1; i <= installmentPlan; i++)
            {
                // Calcular o valor da parcela
                amount = installmentValue;

                // Calcular a data da parcela
                dueDate = purchaseDate.AddMonths(i * monthsInterval);
                string monthYear = dueDate.ToString("MM/yyyy");

                installmentDisplay = i.ToString("00") + "/" + installmentPlan.ToString("00");

                // Create a instance of Installment to Add the new
                Installments installment = new()
                {
                    SpendIdSpend = spendId,
                    InstallmentValue = amount,
                    MonthYear = monthYear,
                    Active = 1,
                    Installment = installmentDisplay
                };
                installments.Add(installment);

                Console.WriteLine("Installment:" + installmentDisplay);
            }

            return installments;
        }

    }
}
