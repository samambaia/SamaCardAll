using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ISpendService
    {
        IEnumerable<Spend> GetSpends();
        Spend GetById(int id);
        void Create(Spend spend);
        void Update(Spend spend);
        void Delete(int id);
    }
}
