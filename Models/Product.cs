namespace NWebApp.Models;

public class Product
{
    public long ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
    public long SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}