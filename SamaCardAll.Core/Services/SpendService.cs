using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {
        private readonly ISpendRepository spendRepository;

        public SpendService(ISpendRepository repository)
        {
            spendRepository = repository;
        }

        public async Task<List<SpendVO>> GetSpendsAsync()
        {
            var result = await spendRepository.GetSpendsAsync();
            return result;
        }

        public async Task<SpendVO> GetByIdAsync(int id)
        {
            return await spendRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(SpendVO spend)
        {
            await spendRepository.CreateAsync(spend);
        }

        public async Task<bool> UpdateAsync(SpendVO spend)
        {
            return await spendRepository.UpdateAsync(spend);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await spendRepository.DeleteAsync(id);
        }
    }
}