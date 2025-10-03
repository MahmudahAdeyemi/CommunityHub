namespace ComuunityHub.Models;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string Title { get; set; }
    public string? Url {get; set;}
    public string Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public User User { get; set; }
}