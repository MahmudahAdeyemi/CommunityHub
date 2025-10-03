using ComuunityHub.Data;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly MyContext _context;

    public OrderRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(string orderId)
    {
        return await _context.Orders
            .Include(x => x.SubOrders)
            .ThenInclude(x => x.SubOrderItems)
            .FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<List<Order>> GetByBuyerIdAsync(string buyerId)
    {
        return await _context.Orders
            .Include(x => x.SubOrders)
            .ThenInclude(x => x.SubOrderItems)
            .Where(x => x.BuyerId == buyerId).ToListAsync();
    }
    public async Task AddOrderAsync(Order order)
    {
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetByPaymentReferenceAsync(string paymentReference)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.PaymentReference == paymentReference);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Order order)
    {
        _context.Remove(order);
        await _context.SaveChangesAsync();
    }

}