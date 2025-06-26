using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository repository)
        {
            customerRepository = repository;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await customerRepository.GetCustomersAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await customerRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Customer customer)
        {
            await customerRepository.CreateAsync(customer);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await customerRepository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await customerRepository.DeleteAsync(id);
        }
    }
}
