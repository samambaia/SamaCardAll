using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<CustomerVO>> GetCustomersAsync();
        Task<CustomerVO> GetByIdAsync(int id);
        Task CreateAsync(CustomerVO customer);
        Task UpdateAsync(CustomerVO customer);
        Task DeleteAsync(int id);
    }
}