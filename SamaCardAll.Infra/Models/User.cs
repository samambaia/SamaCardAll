﻿using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Infra.Models;

public class User
{
    [Key]
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public short Active { get; set; }

}
