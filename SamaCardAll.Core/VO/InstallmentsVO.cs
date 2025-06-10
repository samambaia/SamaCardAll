using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.VO
{
    public record InstallmentsVO(
        int IdInstallment,
        int IdCard,
        string MonthYear,
        decimal InstallmentAmount,
        string InstallmentStatus
    );
}
