using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class Customer
{
    [Key]
    public int IdCustomer { get; set; }
    public string? CustomerName { get; set; }
    // 1 = Active, 0 = Inactive
    public short Active { get; set; }
}