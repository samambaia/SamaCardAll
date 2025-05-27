using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(s => s.IdCustomer == id)
                ?? throw new InvalidOperationException($"Customer with id {id} not found.");
        }

        public async Task CreateAsync(Customer customer)
        {
            customer.IdCustomer = await _context.Customers.MaxAsync(c => c.IdCustomer) + 1;

            _context.Add(customer);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(s => s.IdCustomer == customer.IdCustomer);

            if (existingCustomer != null)
            {
                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.Active = customer.Active;

                _context.SaveChanges();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customerToRemove = await _context.Customers.FirstOrDefaultAsync(s => s.IdCustomer == id);

            if (customerToRemove != null)
            {
                _context.Remove(customerToRemove);
                _context.SaveChanges();
            }
        }
    }
}
