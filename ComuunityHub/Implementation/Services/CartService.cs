using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _productRepository =  productRepository;
    }

    public async Task<BaseResponse> AddItemAsync(string productId, int quantity)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            cart = new Cart()
            {
                UserId = userId
            };
            await _cartRepository.AddCartAsync(cart);
        }
        var product = await _productRepository.GetByIdAsync(productId);
        if (product.Stock < quantity)
        {
            return new BaseResponse()
            {
                Message = "Insufficient stock",
                Status = false
            };
        }
        product.Stock -= quantity;
        await _productRepository.UpdateAsync(product);
        var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            cart.Items.Add( new CartItem()
            {
                ProductId = productId,
                Quantity = quantity
            });
        }
        cart.LastUpdatedAt = DateTime.Now;
        await _cartRepository.UpdateCartAsync(cart);
        return new BaseResponse()
        {
            Message = "Successfully added item",
            Status = true
        };
    }

    public async Task<BaseResponse> RemoveItemAsync(string productId)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            return new BaseResponse()
            {
                Message = "Cart not found",
                Status = false
            };
        }

        var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
        {
            return new BaseResponse()
            {
                Message = "Item not found in cart",
                Status = false
            };
        }

        var product = await _productRepository.GetByIdAsync(productId);
        if (product != null)
        {
            product.Stock += item.Quantity;
        }

        cart.Items.Remove(item);
        cart.LastUpdatedAt = DateTime.Now;
        await _cartRepository.UpdateCartAsync(cart);
        await _productRepository.UpdateAsync(product);
        return new BaseResponse()
        {
            Message = "Successfully removed item",
            Status = true
        };
    }

    public async Task<BaseResponse> IncreaseQuantityAsync(string productId, int quantityChange)
    {
        var userId =  _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (quantityChange <= 0)
        {
            return new BaseResponse()
            {
                Message = "quantityChange must be greater than zero",
                Status = false
            };
        }
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            return new BaseResponse()
            {
                Message = "Cart not found",
                Status = false
            };
        }
        var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
        {
            return new BaseResponse()
            {
                Message = "Item not found in cart",
                Status = false
            };
        }
        item.Quantity += quantityChange;
        var product = await _productRepository.GetByIdAsync(productId);
        if (product.Stock < quantityChange)
        {
            return new BaseResponse()
            {
                Message = "Insufficient stock",
                Status = false
            };
        }

        product.Stock -= quantityChange;
        cart.LastUpdatedAt = DateTime.Now;
        await _productRepository.UpdateAsync(product);
        await _cartRepository.UpdateCartAsync(cart);
        return new BaseResponse()
        {
            Message = "Successfully increased item quantity",
            Status = true
        };
    }

    public async Task<BaseResponse> DecreaseQuantityAsync(string productId, int quantityChange)
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub);
        if (quantityChange <= 0)
        {
            return new BaseResponse()
            {
                Message = "quantityChange must be greater than zero",
                Status = false
            };
        }
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            return new BaseResponse()
            {
                Message = "Cart not found",
                Status = false
            };
        }
        var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (item == null)
        {
            return new BaseResponse()
            {
                Message = "Item not found in cart",
                Status = false
            };
        }
        item.Quantity -= quantityChange;
        var product = await _productRepository.GetByIdAsync(productId);
        if (product.Stock < quantityChange)
        {
            return new BaseResponse()
            {
                Message = "Insufficient stock",
                Status = false
            };
        }

        product.Stock += quantityChange;
        cart.LastUpdatedAt = DateTime.Now;
        await _productRepository.UpdateAsync(product);
        await _cartRepository.UpdateCartAsync(cart);
        return new BaseResponse()
        {
            Message = "Successfully decresed item quantity",
            Status = true
        };
    }

    public async Task<BaseResponse> EmptyCartAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub);
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Cart not found"
            };
        }
        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
        cart.Items.Clear();
        cart.LastUpdatedAt = DateTime.UtcNow;
        await _cartRepository.UpdateCartAsync(cart);

        return new BaseResponse
        {
            Status = true,
            Message = "Cart emptied successfully"
        };
    }
}