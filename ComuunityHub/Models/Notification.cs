namespace ComuunityHub.Models;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public User User { get; set; }
}