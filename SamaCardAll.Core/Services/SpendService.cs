using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Services
{
    public class SpendService : ISpendService
    {
        private readonly ISpendRepository spendRepository;

        public SpendService(ISpendRepository repository)
        {
            spendRepository = repository;
        }

        public async Task<List<Spend>> GetSpendsAsync()
        {
            var result = await spendRepository.GetSpendsAsync();
            return result;
        }

        public async Task<Spend> GetByIdAsync(int id)
        {
            return await spendRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Spend spend)
        {
            await spendRepository.CreateAsync(spend);
        }

        public async Task<bool> UpdateAsync(Spend spend)
        {
            return await spendRepository.UpdateAsync(spend);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await spendRepository.DeleteAsync(id);
        }
    }
}