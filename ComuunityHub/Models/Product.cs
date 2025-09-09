namespace ComuunityHub.Models;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CommunityId { get; set; }
    public string SellerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public Community Community { get; set; }
    public Seller Seller { get; set; }
    public ICollection<Order> Orders { get; set; }
}