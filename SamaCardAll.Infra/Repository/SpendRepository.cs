using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using System.Net.WebSockets;

namespace SamaCardAll.Infra.Repository
{
    public class SpendRepository : ISpendRepository
    {
        private readonly AppDbContext _context;
        private readonly List<Installments> installmentsExist;

        public SpendRepository(AppDbContext context)
        {
            _context = context;

            var q = _context.Installments
                .Include(s => s.Spend);

            installmentsExist = [..q];
        }

        public async Task CreateAsync(Spend spend)
        {
            // TODO: Atribuir manualmente o idUser, enquanto não tem um serviço para recuperar o usuário logado.
            spend.UserIdUser = 1;

            // Add a new expense
            _context.Spends.Add(spend);

            // Generate Installment Plan
            var installmentList = GenerateInstallmentPlan(
                                    spend, 
                                    spend.InstallmentPlan, 
                                    spend.InstallmentValue, 
                                    spend.Date);

            // Add Installment List
            _context.Installments.AddRange(installmentList); 

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var spend = await _context.Spends.FindAsync(id);
            if (spend is null) return false;

            spend.Deleted = 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Spend> GetByIdAsync(int id)
        {
            return await _context.Spends.FindAsync(id);
        }

        public async Task<List<Spend>> GetSpendsAsync()
        {
            var getSpends = await _context.Spends
                                          .Include(s => s.Customer)
                                          .Include(s => s.Card)
                                          .Include(s => s.User)
                                          .ToListAsync();

            return [.. getSpends.Select(s => s)];
        }

        public async Task<bool> UpdateAsync(Spend spend)
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

        private List<Installments> GenerateInstallmentPlan(Spend spend, int installmentPlan, decimal installmentValue, DateTime purchaseDate)
        {
            var installmentsLocal = new List<Installments>();

            // Add installments to the installment plan
            for (int i = 1; i <= installmentPlan; i++)
            {
                var dueDate = purchaseDate.AddMonths(i);
                var display = $"{i:00}/{installmentPlan:00}";

                installmentsLocal.Add(new Installments
                {
                    Spend = spend,
                    InstallmentValue = installmentValue,
                    MonthYear = dueDate.ToString("MM/yyyy"),
                    Installment = display,
                    Active = 1
                });

                Console.WriteLine("Installment:" + display);
            }

            return installmentsLocal;
        }
    }
}
