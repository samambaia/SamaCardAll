using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models
{
    public class Installments
    {
        [Key]
        public int Id { get; set; }
        public string MonthYear { get; set; }
        public decimal InstallmentValue { get; set; }
        public short Active { get; set; } = 1;

        //Spend Relationship
        public int SpendIdSpend { get; set; }
        public Spend? Spend { get; set; }
    }
}
