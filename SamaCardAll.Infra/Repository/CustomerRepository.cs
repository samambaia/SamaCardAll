using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Infra.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContext;
        private readonly int _userId;

        public CustomerRepository(AppDbContext context, IUserContextService userContext)
        {
            _context = context;
            _userContext = userContext;
            _userId = _userContext.GetUserId();
        }

        public async Task CreateAsync(Customer customer)
        {
            customer.UserIdUser = _userId;

            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        { 
            var customer = await GetByIdAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0).Result; // Ensure the operation is successful
            }
            return false;
        }

        public async Task<List<Customer>> GetActiveCustomersAsync()
        {
            var activeCustomers =  await _context.Customers
                .Where(c  => c.Active == 1 && c.UserIdUser == _userId)
                .ToListAsync();

            return [.. activeCustomers.Select(c => c)];
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.UserIdUser == _userId)
                .FirstOrDefaultAsync(c => c.IdCustomer == id);
            return await _context.Customers.FindAsync(id);
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers = await _context.Customers
                .Where(c => c.UserIdUser == _userId)
                .ToListAsync();

            return [.. customers.Select(c => c)];
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.IdCustomer);

            if (existingCustomer == null)
                return false;

            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.Active = customer.Active;
            existingCustomer.UserIdUser = customer.UserIdUser;

            _context.SaveChanges();

            return true;
        }
    }
}
