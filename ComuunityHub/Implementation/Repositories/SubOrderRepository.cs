using ComuunityHub.Data;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class SubOrderRepository : ISubOrderRepository
{
    private readonly MyContext _context;
    public SubOrderRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<SubOrder> GetById(string id)
    {
        return await _context.SubOrders
            .Include(x => x.SubOrderItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<SubOrder>> GetByOrderId(string orderId)
    {
        return await _context.SubOrders
            .Include(x => x.SubOrderItems)
            .Where(x => x.OrderId == orderId).ToListAsync();
    }
    public async Task AddAsync(SubOrder subOrder)
    {
        await _context.SubOrders.AddAsync(subOrder);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SubOrder>> GetBySellerId(string sellerId)
    {
        return await _context.SubOrders.Where(x => x.SellerId == sellerId).ToListAsync();
    }
    
    public async Task UpdateAsync(SubOrder subOrder)
    {
        _context.SubOrders.Update(subOrder);
        await _context.SaveChangesAsync();
    }
}