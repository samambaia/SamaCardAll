using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models
{
    public class Installments
    {
        [Key]
        public int Id { get; set; }
        public string MonthYear { get; set; }
        public decimal InstallmentValue { get; set; }
        public short Active { get; set; }

        //Spend Relationship
        public int IdSpend { get; set; }
        public Spend? Spend { get; set; }
    }
}
