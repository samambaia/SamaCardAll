using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.DTO
{
    public record CardVO(int IdCard, string Bank, string Number, string Expiration, string Brand, short Active);
}
