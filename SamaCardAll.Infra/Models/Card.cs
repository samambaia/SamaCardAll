using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamaCardAll.Infra.Models;

public class Card
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCard { get; set; }
    [Required]
    public string? Bank { get; set; }
    [Required]
    public string? Number { get; set; }
    [Required]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/([1-9]|[1-9][0-9])$", ErrorMessage = "Incorrect Expiration Date!")]
    public string? Expiration { get; set; }
    [Required]
    public string? Brand { get; set; } = "Mastercard";
    // 1 = Active, 0 = Inactive
    public short Active {  get; set; }

}
