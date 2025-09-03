namespace ComuunityHub.Models;

public class Provider
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Skills { get; set; }
    public double Rating { get; set; }
    public User User { get; set; }
    public ICollection<Service> Services { get; set; }
}