namespace ComuunityHub.Models;

public class CommunityMember
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string CommunityId { get; set; }
    public Community Community { get; set; }
    public Roles Role { get; set; }
    public DateTime JoinedAt { get; set; } =  DateTime.Now;
}