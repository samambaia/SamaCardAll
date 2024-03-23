using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {

        private readonly AppDbContext _context;
        private readonly List<Spend> _spends;

        public SpendService(AppDbContext context)
        {
            _context = context;
            _spends = _context.Spends.ToList(); // Initialize _spends here
        }

        public IEnumerable<Spend> GetSpends()
        {
            // Retorna todos os gastos
            return _spends;
        }

        public Spend GetById(int id)
        {
            // Busca um gasto pelo ID na lista de gastos
            return _spends.FirstOrDefault(s => s.IdSpend == id);
        }

        public void Create(Spend spend)
        {
            // Define o ID do gasto (poderia ser gerado automaticamente em um banco de dados real)
            spend.IdSpend = _spends.Count + 1;

            // Adiciona o novo gasto à lista de gastos
            _spends.Add(spend);
        }

        public void Update(Spend spend)
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

        public void Delete(int id)
        {
            // Remove o gasto com o ID especificado da lista de gastos
            var spendToRemove = _spends.FirstOrDefault(s => s.IdSpend == id);
            if (spendToRemove != null)
            {
                _spends.Remove(spendToRemove);
                _context.SaveChanges();
            }
        }
    }
}
