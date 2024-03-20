using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class Card
{
    [Key]
    public int IdCard { get; set; }
    public string? Bank { get; set; }
    public string? Number { get; set; }
    public string? Expiration { get; set; }
    public string? Brand { get; set;}
    public short Active {  get; set; }

}
