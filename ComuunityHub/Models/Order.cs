namespace ComuunityHub.Models;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BuyerId { get; set; }
    public string PaymentId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.Now;
    public Buyer Buyer { get; set; }
    public Product Product { get; set; }
    public Payment Payment { get; set; }
}