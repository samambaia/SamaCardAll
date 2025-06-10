using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.DTO
{
    public record TotalCardMonthYearDTO(int IdCard, string CardName, decimal InstallmentAmount, string MonthYear) : IEquatable<TotalCardMonthYearDTO>;
}
