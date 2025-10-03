using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly MyContext _context;

    public SellerRepository(MyContext context)
    {
        _context = context;
    }
    public async Task<Seller> AddAsync(Seller seller)
    {
        await _context.Sellers.AddAsync(seller);
        await _context.SaveChangesAsync();
        return seller;
    }
    public async Task<ICollection<Seller>> GetByUserAsync(string userId)
    {
        return await _context.Sellers
            .Include(s => s.Community)
            .Include(s => s.Address)
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }
    public async Task<Seller?> GetByIdAsync(string sellerId)
    {
        return await _context.Sellers
            .Include(s => s.User)
            .Include(s => s.Community)
            .FirstOrDefaultAsync(s => s.Id == sellerId);
    }
    public async Task<Seller?> GetByUserAndCommunityAsync(string userId, string communityId)
    {
        return await _context.Sellers
            .Include(s => s.User)
            .Include(s => s.Community)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.CommunityId == communityId);
    }
    public async Task<List<Seller>> GetByCommunityAsync(string communityId)
    {
        return await _context.Sellers
            .Include(s => s.User)
            .Where(s => s.CommunityId == communityId)
            .ToListAsync();
    }
    public async Task DeleteAsync(Seller seller)
    {
        _context.Sellers.Remove(seller);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seller seller)
    {
        _context.Sellers.Update(seller);
        await _context.SaveChangesAsync();
    }
}