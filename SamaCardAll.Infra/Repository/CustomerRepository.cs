using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Mapping;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CustomerVO customerVO)
        {
            var customer = new Customer
            {
                CustomerName = customerVO.CustomerName,
                Active = customerVO.Active
            };

            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        { 
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0).Result; // Ensure the operation is successful
            }
            return false;
        }

        public async Task<CustomerVO> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.IdCustomer == id)
                .Select(c => c.ToVO())
                .FirstOrDefaultAsync();

            return customer;
        }

        public async Task<List<CustomerVO>> GetCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            return [.. customers.Select(c => c.ToVO()];
        }

        public async Task<bool> UpdateAsync(CustomerVO customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.IdCustomer);

            if (existingCustomer != null)
            {
                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.Active = customer.Active;

                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
