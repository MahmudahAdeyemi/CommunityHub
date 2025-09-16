using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class CommunityMemberRepository : ICommunityMemberRepository
{
    private readonly MyContext _context;

    public CommunityMemberRepository(MyContext context)
    {
        _context = context;
    }
    public async Task<CommunityMember?> GetMembershipAsync(string communityId, string userId)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId);
    }
    public async Task AddCommunityMember(CommunityMember communityMember)
    {
        await _context.CommunityMembers.AddAsync(communityMember);
        await _context.SaveChangesAsync();
    }
    public async Task<CommunityMember?> GetMemberByIdAsync(string communityId, string userId)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId);
    }
    public async Task RemoveMemberAsync(CommunityMember member)
    {
        _context.CommunityMembers.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMemberAsync(CommunityMember member)
    {
        _context.CommunityMembers.Update(member);
        await _context.SaveChangesAsync();
    }
    
}