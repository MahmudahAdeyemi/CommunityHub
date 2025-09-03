namespace ComuunityHub.Models;

public class Job
{
    public string Id { get; set; }
    public string CommunityId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.Now;
    public Community Community { get; set; }
    public Employer Employer { get; set; }
    public ICollection<Application> Applications { get; set; }
}