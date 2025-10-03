namespace ComuunityHub.Models;

public class CommunityMemberRole
{
    public string Id = Guid.NewGuid().ToString();
    public string CommunityMemberId { get; set; }
    public CommunityMember CommunityMember { get; set; }
    public CommunityRole Role { get; set; }
}