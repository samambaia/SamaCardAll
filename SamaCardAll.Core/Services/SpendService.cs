using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {
        private readonly AppDbContext _context;
        private readonly List<Installments> _installmentsExist;
        private readonly List<Installments> _installments = new();

        private readonly int MaxId = 0;

        public SpendService(AppDbContext context)
        {
            _context = context;

            // Initialize Installment
            var q = _context.Installments
                .Include(s => s.Spend);

            _installmentsExist = q.ToList();
        }

        public async Task<IEnumerable<Spend>> GetSpendsAsync()
        {
            // Return IQueryable to enable further querying
            return await _context.Spends.ToListAsync();
        }

        public async Task<Spend> GetByIdAsync(int id)
        {
            // Search for an expense by ID in the expense list
            return await _context.Spends.FirstOrDefaultAsync(s => s.IdSpend == id)
                ?? throw new InvalidOperationException($"Spend with id {id} not found.");
        }

        public async Task CreateAsync(Spend spend)
        {
            // Define de ID
            spend.IdSpend = await _context.Spends.AnyAsync() ? await _context.Spends.MaxAsync(s => s.IdSpend) + 1 : 1;

            spend.UserIdUser = 1;
            spend.CreatedDate = DateTime.Now;

            // Generate the Installment List
            var installmentList = GenerateInstallmentPlan(spend.IdSpend, spend.InstallmentPlan, spend.InstallmentValue, spend.Date);

            // Add a new expense
            _context.Add(spend);
            // Add Installment List
            _context.Installments.AddRange(installmentList);

            _context.SaveChanges();
        }

        public async Task UpdateAsync(Spend spend)
        {
            // Busca o gasto pelo ID na lista de gastos
            var existingSpend = await _context.Spends.FirstOrDefaultAsync(s => s.IdSpend == spend.IdSpend);

            if (existingSpend != null)
            {
                // Atualiza os campos do gasto existente com os valores do novo gasto
                existingSpend.Expenses = spend.Expenses;
                existingSpend.Amount = spend.Amount;
                existingSpend.Date = spend.Date;
                existingSpend.InstallmentPlan = spend.InstallmentPlan;
                existingSpend.InstallmentValue = spend.InstallmentValue;
                existingSpend.Deleted = spend.Deleted;
                existingSpend.CreatedDate = spend.CreatedDate;
                existingSpend.CardIdCard = spend.CardIdCard;
                existingSpend.CustomerIdCustomer = spend.CustomerIdCustomer;
                existingSpend.UserIdUser = spend.UserIdUser;

                _context.SaveChanges();
            }
        }

        public async Task DeleteAsync(int id)
        {
            // Remove o gasto com o ID especificado da lista de gastos
            var spendToRemove = await _context.Spends.FirstOrDefaultAsync(s => s.IdSpend == id);
            if (spendToRemove != null)
            {
                spendToRemove.Deleted = 1;

                //_context.Remove(spendToRemove); Logical Delete
                _context.SaveChanges();
            }
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
            if (_installmentsExist.Count > 0)
            {
                maxId = _installmentsExist.Max(s => s.Id) + 1;
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
                _installments.Add(installment);

                Console.WriteLine("Installment:" + installmentDisplay);
            }

            return _installments;
        }
    }
}