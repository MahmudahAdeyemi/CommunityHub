namespace ComuunityHub.Models;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BuyerId { get; set; }
    public string AddressId { get; set; }
    public string? PaymentReference { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public Address DeliveryAddress { get; set; }
    public ICollection<SubOrder> SubOrders { get; set; } = [];
    public Buyer Buyer { get; set; }
}