using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaCardAll.Infra.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCustomer { get; set; }
    public string? CustomerName { get; set; }
    // 1 = Active, 0 = Inactive
    public short Active { get; set; }
}