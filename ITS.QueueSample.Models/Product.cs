using System.ComponentModel.DataAnnotations;

namespace ITS.QueueSample.Models;

// specifies table for dapper contrib
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
