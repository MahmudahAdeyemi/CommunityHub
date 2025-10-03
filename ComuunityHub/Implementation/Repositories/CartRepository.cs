using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class CartRepository : ICartRepository
{
    private readonly MyContext _context;
    public CartRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserId(string userId)
    {
        return await _context.Carts.Include(c => c.Items)
            .ThenInclude(c => c.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task EmptyCartAsync(Cart cart)
    {
         _context.Carts.First(x => x.UserId == cart.UserId).Items.Clear();
         await _context.SaveChangesAsync();
    }

    public async Task<List<Cart>> GetExpiredCarts()
    {
        return _context.Carts.Where(x => x.LastUpdatedAt <= DateTime.Now.AddDays(-30)).ToList();
    }
}