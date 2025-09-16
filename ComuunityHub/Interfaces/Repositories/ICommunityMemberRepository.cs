using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ICommunityMemberRepository
{
    Task AddCommunityMember(CommunityMember communityMember);
    Task<CommunityMember?> GetMemberByIdAsync(string communityId, string userId);
    Task RemoveMemberAsync(CommunityMember member);
    Task<CommunityMember?> GetMembershipAsync(string communityId, string userId);
    Task UpdateMemberAsync(CommunityMember member);
}