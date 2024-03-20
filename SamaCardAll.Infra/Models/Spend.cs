using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class Spend
{
    [Key]
    public int IdSpend { get; set; }
    public string? Expenses { get; set;}
    public decimal Amount { get; set;}
    public DateTime Date { get; set; } = DateTime.Now;
    public short InstallmentPlan { get; set; } = 1;
    public decimal InstallmentValue { get; set;}
    public short Deleted { get; set;}
    public DateTime CreatedDate { get; set;}

    //Customer
    public Customer? Customer { get; set;}

    //Card
    public Card? Card { get; set;}

    //User
    public User? User  { get; set;}

}
