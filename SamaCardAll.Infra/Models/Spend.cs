using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class Spend
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdSpend { get; set; }
    public string Expenses { get; set;}
    public decimal Amount { get; set;}
    public DateTime Date { get; set; } = DateTime.Now;
    public int InstallmentPlan { get; set; } = 1;
    public decimal InstallmentValue { get; set;}
    public short Deleted { get; set;}
    public DateTime CreatedDate { get; set;} = DateTime.Now;

    //Customer
    public int CustomerIdCustomer { get; set; }
    public Customer Customer { get; set;}

    //Card
    public int CardIdCard { get; set; }
    public Card Card { get; set;}

    //User
    public int UserIdUser { get; set; } = 1; // Default to 1 for the admin user
    public User User  { get; set;}

}
