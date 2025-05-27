using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {
        private readonly AppDbContext _context;
        private readonly List<Spend> _spends;
        private readonly List<Installments> _installmentsExist;

        private List<Installments> _installments = new List<Installments>();

        private int MaxId = 0;

        public SpendService(AppDbContext context)
        {
            _context = context;

            // Initialize Spend
            var query = _context.Spends
                .Where(s => s.Deleted == 0) // Filter only non Deleted Records
                .Include(s => s.Card)
                .Include(s => s.Customer)
                .Include(s => s.User);

            _spends = query.ToList();

            // Initialize Installment
            var q = _context.Installments
                .Include(s => s.Spend);

            _installmentsExist = q.ToList();

            // Initialize Spend
            MaxId = (_context.Spends != null && _context.Spends.Any()) ? _context.Spends.Max(s => s.IdSpend) : 0;
        }

        public IEnumerable<Spend> GetSpends()
        {
            // Return IQueryable to enable further querying
            return _spends;
        }

        Spend ISpendService.GetById(int id)
        {
            // Search for an expense by ID in the expense list
            return _spends.FirstOrDefault(s => s.IdSpend == id);
        }

        void ISpendService.Create(Spend spend)
        {
            // Define de ID
            if (MaxId > 0)
            {
                spend.IdSpend = ++MaxId;
            }
            else
                spend.IdSpend = 1;

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

        // Create Installment List
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
                Installments installment = new Installments
                {
                    SpendIdSpend = spendId,
                    //Id = maxId++,
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

        void ISpendService.Update(Spend spend)
        {
            // Busca o gasto pelo ID na lista de gastos
            var existingSpend = _spends.FirstOrDefault(s => s.IdSpend == spend.IdSpend);

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

        void ISpendService.Delete(int id)
        {
            // Remove o gasto com o ID especificado da lista de gastos
            var spendToRemove = _spends.FirstOrDefault(s => s.IdSpend == id);
            if (spendToRemove != null)
            {
                spendToRemove.Deleted = 1;

                //_context.Remove(spendToRemove); Logical Delete
                _context.SaveChanges();
            }
        }
    }
}