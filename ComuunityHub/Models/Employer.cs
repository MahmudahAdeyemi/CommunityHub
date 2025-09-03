namespace ComuunityHub.Models;

public class Employer
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string CompanyName { get; set; }
    public User User { get; set; }
    public ICollection<Job> Jobs { get; set; }
}