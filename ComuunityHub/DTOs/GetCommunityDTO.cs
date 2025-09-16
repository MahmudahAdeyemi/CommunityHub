using ComuunityHub.Models;

namespace ComuunityHub.DTOs;

public record GetCommunityDTO
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string Status { get; init; }
    public int NoOfMembers { get; init; }
    public DateTime Created { get; init; }
    public List<CommunityMember> Members { get; init; } = [];
}