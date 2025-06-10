using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.VO
{
    public record CustomerVO(int IdCustomer, string CustomerName, short Active);
}
