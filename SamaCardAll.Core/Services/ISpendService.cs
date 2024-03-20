using System;
using System.Collections.Generic;
using SamaCardAll.Core.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.Services
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
