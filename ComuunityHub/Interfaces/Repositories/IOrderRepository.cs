using ComuunityHub.Models;

namespace ComuunityHub.Implementation.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(string orderId);
    Task<List<Order>> GetByBuyerIdAsync(string buyerId);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Order order);
    Task<Order?> GetByPaymentReferenceAsync(string paymentReference);
}