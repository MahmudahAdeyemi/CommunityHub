namespace ComuunityHub.Models;

public class Community
{
    public string Id { get; set; }
    public string Name { get; set; }
    public User CreatedBy { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CommunityMember> Members { get; set; } = [];
}