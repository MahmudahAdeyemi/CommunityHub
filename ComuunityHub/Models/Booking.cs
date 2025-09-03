namespace ComuunityHub.Models;

public class Booking
{
    public string Id { get; set; }
    public string ServiceId { get; set; }
    public string ClientId { get; set; }
    public BookingStatus Status { get; set; } =  BookingStatus.Pending;
    public DateTime Created { get; set; } = DateTime.Now;
    public Service Service { get; set; }
    public User Client { get; set; }
}