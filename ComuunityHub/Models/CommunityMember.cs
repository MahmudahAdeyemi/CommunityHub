namespace ComuunityHub.Models;

public class CommunityMember
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public User User { get; set; }
    public string CommunityId { get; set; }
    public Community Community { get; set; }
    public List<Role> Role { get; set; } = [];
    public CommunityStatus Status { get; set; }
    public CommunityRole CommunityRole { get; set; }
    public DateTime JoinedAt { get; set; } =  DateTime.Now;
}