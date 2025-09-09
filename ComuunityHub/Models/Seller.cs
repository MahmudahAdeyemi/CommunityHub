namespace ComuunityHub.Models;

public class Seller
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string StoreName { get; set; }
    public double Ratings { get; set; }
    public User User { get; set; }
    public ICollection<Product> Products { get; set; }
}