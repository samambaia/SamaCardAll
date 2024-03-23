using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    internal interface ICustomerService
    {
        IEnumerable<Customer> GetCustomer();
        Customer GetById(int id);
        void Create(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
    }
}
