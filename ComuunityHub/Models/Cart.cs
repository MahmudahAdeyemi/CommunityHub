namespace ComuunityHub.Models;

public class Cart
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}