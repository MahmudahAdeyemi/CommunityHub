namespace ComuunityHub.Models;

public class SubOrderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SubOrderId { get; set; }
    public SubOrder SubOrder { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}