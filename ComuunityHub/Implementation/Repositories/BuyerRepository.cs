using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class BuyerRepository : IBuyerRepository
{
    private readonly MyContext _context;

    public BuyerRepository(MyContext context)
    {
        _context = context;
    }
    public async Task<Buyer?> GetByIdAsync(string id)
    {
        return await _context.Buyers
            .Include(b => b.User) 
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    public async Task<Buyer?> GetByUserIdAsync(string userId)
    {
        return await _context.Buyers
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }
    public async Task AddAsync(Buyer buyer)
    {
        await _context.Buyers.AddAsync(buyer);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Buyer buyer)
    {
        _context.Buyers.Update(buyer);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(Buyer buyer)
    {
        _context.Buyers.Remove(buyer);
        await _context.SaveChangesAsync();
        return true;
    }
}