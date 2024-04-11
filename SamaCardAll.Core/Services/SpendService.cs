using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.VisualBasic;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {

        private readonly AppDbContext _context;
        private readonly List<Spend> _spends;
        private List<Installments> _installments = new List<Installments>();

        public SpendService(AppDbContext context)
        {
            _context = context;

            // Initialize Spend
            //_spends = _context.Spends.ToList(); 
            var query = _context.Spends
                .Include(s => s.Card)
                .Include(s => s.Customer);

            _spends = query.ToList();
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
            spend.IdSpend = _spends.Max(s => s.IdSpend) + 1;
            spend.UserIdUser = 1;

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
            int maxId = 1;
            // Calculate Installment ID
            if (_installments.Count > 0)
            {
                maxId = _installments.Max(s => s.Id);
            }

            // Calcular o intervalo de meses entre cada parcela
            int monthsInterval = 1;

            decimal amount = 0;
            DateTime dueDate;

            // Adicionar parcelas ao plano de parcelamento
            for (int i = 1; i <= installmentPlan; i++)
            {
                // Calcular o valor da parcela
                amount = installmentValue;

                // Calcular a data da parcela
                dueDate = purchaseDate.AddMonths(i * monthsInterval);
                string monthYear = dueDate.ToString("MM/yyyy");

                // Criar uma instância de Installment e adicioná-la à lista
                Installments installment = new Installments
                {
                    SpendIdSpend = spendId,
                    Id = maxId++,
                    InstallmentValue = amount,
                    MonthYear = monthYear,
                    Active = 1
                };
                _installments.Add(installment);
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

                // Atualiza o Customer
                if (spend.Customer != null)
                {
                    existingSpend.Customer = new Customer
                    {
                        IdCustomer = spend.Customer.IdCustomer, // Supondo que IdCustomer seja a chave primária de Customer
                                                                // Atualize outras propriedades de Customer conforme necessário
                    };
                }

                // Atualiza o Card
                if (spend.Card != null)
                {
                    existingSpend.Card = new Card
                    {
                        IdCard = spend.Card.IdCard, // Supondo que IdCard seja a chave primária de Card
                                                    // Atualize outras propriedades de Card conforme necessário
                    };
                }

                // Atualiza o User
                if (spend.User != null)
                {
                    existingSpend.User = new User
                    {
                        IdUser = spend.User.IdUser, // Supondo que IdUser seja a chave primária de User
                                                    // Atualize outras propriedades de User conforme necessário
                    };
                }

                _context.SaveChanges();
            }
        }

        void ISpendService.Delete(int id)
        {
            // Remove o gasto com o ID especificado da lista de gastos
            var spendToRemove = _spends.FirstOrDefault(s => s.IdSpend == id);
            if (spendToRemove != null)
            {
                _context.Remove(spendToRemove);
                _context.SaveChanges();
            }
        }
    }
}
