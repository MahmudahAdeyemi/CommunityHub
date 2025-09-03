namespace ComuunityHub.Models;

public class JobSeeker
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string CVLink { get; set; }
    public User User { get; set; }
    public ICollection<Application> Applications { get; set; }
}