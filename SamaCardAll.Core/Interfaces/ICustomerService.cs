using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetById(int id);
        void Create(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
    }
}
