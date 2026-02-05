namespace ErpAcl.Domain.Models;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

