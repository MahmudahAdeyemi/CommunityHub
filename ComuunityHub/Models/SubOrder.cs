namespace ComuunityHub.Models;

public class SubOrder
{
    public string Id { get; set; } =  Guid.NewGuid().ToString();
    public string OrderId { get; set; }
    public Order Order { get; set; }
    public SubOrderStatus SubOrderStatus { get; set; }
    public string SellerId { get; set; }
    public User Seller { get; set; }
    public List<SubOrderItem> SubOrderItems { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    
}