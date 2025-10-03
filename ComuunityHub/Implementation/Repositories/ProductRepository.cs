using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MyContext _context;

    public ProductRepository(MyContext context)
    {
        _context = context;
    }
    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task<Product?> GetByIdAsync(string productId)
    {
        return await _context.Products
            .Include(p => p.Seller)
            .ThenInclude(p => p.User)
            .Include(p => p.Community)
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
    public async Task<List<Product>> GetByCommunityAsync(string communityId)
    {
        return await _context.Products
            .Include(p => p.Seller)
            .Where(p => p.CommunityId == communityId)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetBySellerAsync(string sellerId)
    {
        return await _context.Products
            .Include(p => p.Community)
            .Where(p => p.SellerId == sellerId)
            .ToListAsync();
    }
    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }
    
    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}