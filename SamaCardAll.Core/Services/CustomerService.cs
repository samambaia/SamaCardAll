using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    internal class CustomerService : ICustomerService
    {

        private readonly AppDbContext _context;
        private readonly List<Customer> _customers;

        public CustomerService(AppDbContext context)
        {
            _context = context;

            // Initialize Customer
            _customers = _context.Customers.ToList();
        }


        IEnumerable<Customer> ICustomerService.GetCustomer()
        {
            return _customers;
        }

        Customer ICustomerService.GetById(int id)
        {
            return _customers.FirstOrDefault(s => s.IdCustomer == id);
        }

        void ICustomerService.Create(Customer customer)
        {
            customer.IdCustomer = _customers.Count + 1;

            _customers.Add(customer);
        }

        void ICustomerService.Update(Customer customer)
        {
            var existingCustomer = _customers.FirstOrDefault(s => s.IdCustomer == customer.IdCustomer);

            if (existingCustomer != null)
            {
                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.Active = customer.Active;

                _context.SaveChanges();
            }
        }

        void ICustomerService.Delete(int id)
        {
            var customerToRemore = _customers.FirstOrDefault(s => s.IdCustomer == id);
            if (customerToRemore != null)
            {
                _customers.Remove(customerToRemore);
                _context.SaveChanges();
            }
        }

    }
}
