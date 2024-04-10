using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {

        private readonly AppDbContext _context;
        private readonly List<Spend> _spends;

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

            // Add a new expense
            _context.Add(spend);
            _context.SaveChanges();
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
