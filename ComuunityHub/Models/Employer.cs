namespace ComuunityHub.Models;

public class Employer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string CompanyName { get; set; }
    public User User { get; set; }
    public ICollection<Job> Jobs { get; set; }
}