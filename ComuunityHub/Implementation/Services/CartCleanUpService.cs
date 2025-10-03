using ComuunityHub.Interfaces.Repositories;

namespace ComuunityHub.Implementation.Services;

public class CartCleanUpService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public CartCleanUpService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var cartRepository = scope.ServiceProvider.GetRequiredService<ICartRepository>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var expiredCarts =await cartRepository.GetExpiredCarts();
                foreach (var cart in expiredCarts)
                {
                    foreach (var item in cart.Items)
                    {
                        var product = await productRepository.GetByIdAsync(item.ProductId);
                        product.Stock += item.Quantity;
                        await productRepository.UpdateAsync(product);
                    }
                    cart.LastUpdatedAt = DateTime.UtcNow;
                    await cartRepository.EmptyCartAsync(cart);
                    cartRepository.UpdateCartAsync(cart);
                }
            }
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}