namespace ComuunityHub.Models;

public class Application
{
    public string Id { get; set; }
    public string JobId { get; set; }
    public string JobSeekerId { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public DateTime AppliedAt { get; set; } = DateTime.Now;
    public Job Job { get; set; }
    public JobSeeker JobSeeker { get; set; }

}