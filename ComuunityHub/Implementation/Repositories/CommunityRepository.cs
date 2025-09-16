using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class CommunityRepository : ICommunityRepository
{
    private readonly MyContext _context;

    public CommunityRepository(MyContext context)
    {
        _context = context;
    }

    public async Task CreateCommunity(Community community)
    {
        await _context.Communities.AddAsync(community);
        await _context.SaveChangesAsync();
    }
    public async Task<Community?> GetByIdAsync(string id)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<List<Community>> GetAllAsync()
    {
        return await _context.Communities
            .Include(c => c.Members)
            .ToListAsync();
    }
    public async Task<IEnumerable<Community>> GetUserCommunity(string userId)
    {
        return await _context.CommunityMembers
            .Where(cm => cm.UserId == userId)
            .Select(cm => cm.Community)
            .ToListAsync();
    }

    public async Task<Community?> GetCommunityByName(string name)
    {
        return await _context.Communities.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task UpdateCommunity(Community community)
    {
        _context.Communities.Update(community);
        await _context.SaveChangesAsync();
    }
}