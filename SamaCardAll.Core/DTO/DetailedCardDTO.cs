using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamaCardAll.Core.DTO
{
    public class DetailedCardDTO
    {
        public int IdCard { get; set; }
        public string CardName { get; set; }
        public string DescriptionSpend { get; set; }
        public string CustomerName { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string MonthYear { get; set; }
        public string Installment { get; set; }
    }
}
