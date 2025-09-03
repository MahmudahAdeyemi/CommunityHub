namespace ComuunityHub.Models;

public class Order
{
    public string OrderId { get; set; }
    public string BuyerId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.Now;
    public Buyer Buyer { get; set; }
    public Product Product { get; set; }
    public Payment Payment { get; set; }
}