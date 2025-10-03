using ComuunityHub.Models;

namespace ComuunityHub.Implementation.Repositories;

public interface ISubOrderRepository
{
    Task<SubOrder> GetById(string id);
    Task<List<SubOrder>> GetByOrderId(string orderId);
    Task AddAsync(SubOrder subOrder);
    Task<List<SubOrder>> GetBySellerId(string sellerId);
    Task UpdateAsync(SubOrder subOrder);
}