using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerVO>> GetCustomersAsync();
        Task<CustomerVO> GetByIdAsync(int id);
        Task CreateAsync(CustomerVO customer);
        Task<bool> UpdateAsync(CustomerVO customer);
        Task<bool> DeleteAsync(int id);
    }
}