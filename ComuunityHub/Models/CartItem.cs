namespace ComuunityHub.Models;

public class CartItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CartId { get; set; }
    public Cart Cart { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    
}