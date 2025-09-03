namespace ComuunityHub.Models;

public class Buyer
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<Order> Orders { get; set; }
}