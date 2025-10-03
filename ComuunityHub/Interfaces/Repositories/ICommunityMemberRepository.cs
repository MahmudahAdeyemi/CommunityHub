using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ICommunityMemberRepository
{
    Task AddCommunityMember(CommunityMember communityMember);
    
    Task RemoveMemberAsync(CommunityMember member);
    Task<CommunityMember?> GetMembershipAsync(string communityId, string userId);
    Task UpdateMemberAsync(CommunityMember member);
    Task<List<CommunityMember>> GetPendingMembers(string communityId);
}