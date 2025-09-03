namespace ComuunityHub.Models;

public class Service
{
    public string Id { get; set; }
    public string CommunityId { get; set; }
    public string ProviderId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Community Community { get; set; }
    public Provider Provider { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}