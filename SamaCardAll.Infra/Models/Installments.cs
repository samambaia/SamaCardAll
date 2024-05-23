using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaCardAll.Infra.Models
{
    public class Installments
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MonthYear { get; set; }
        public decimal InstallmentValue { get; set; }
        public string Installment { get; set; }
        public short Active { get; set; } = 1;

        //Spend Relationship
        public int SpendIdSpend { get; set; }
        public Spend? Spend { get; set; }
    }
}
