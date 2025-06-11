using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository repository)
        {
            customerRepository = repository;
        }

        public async Task<List<CustomerVO>> GetCustomersAsync()
        {
            return await customerRepository.GetCustomersAsync();
        }

        public async Task<CustomerVO> GetByIdAsync(int id)
        {
            return await customerRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(CustomerVO customer)
        {
            await customerRepository.CreateAsync(customer);
        }

        public async Task<bool> UpdateAsync(CustomerVO customer)
        {
            return await customerRepository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await customerRepository.DeleteAsync(id);
        }
    }
}
