﻿using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Core.Models;

public class Customer
{
    [Key]
    public int IdCustomer { get; set; }
    public string? CustomerName { get; set; }
    public short Active { get; set; }
}