using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra.Mapping
{
    public static class CustomerMappingExtensions
    {
        public static CustomerVO ToVO(this Customer customer)
        {
            if (customer == null) return null;
            return new CustomerVO(IdCustomer: customer.IdCustomer,
                CustomerName: customer.CustomerName,
                Active: customer.Active);
        }

        public static Customer ToModel(this CustomerVO customerVO)
        {
            if (customerVO == null) return null;
            return new Customer
            {
                IdCustomer = customerVO.IdCustomer,
                CustomerName = customerVO.CustomerName,
                Active = customerVO.Active
            };
        }
    }
}
