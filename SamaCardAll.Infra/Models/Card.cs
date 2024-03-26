using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class Card
{
    [Key]
    public int IdCard { get; set; }
    [Required]
    public string? Bank { get; set; }
    [Required]
    public string? Number { get; set; }
    public string? Expiration { get; set; }
    [Required]
    public string? Brand { get; set;}
    public short Active {  get; set; }

}
