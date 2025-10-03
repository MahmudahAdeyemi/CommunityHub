using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ComuunityHub.DTOs;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository  _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISellerRepository _sellerRepository;

    public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, ISellerRepository sellerRepository)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _sellerRepository = sellerRepository;
    }

    public async Task<BaseResponse> AddProduct(ProductRequestModel request,string communityId)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var seller =await _sellerRepository.GetByUserAndCommunityAsync(userId, communityId);
        if (seller.Status != SellerStatus.Approved)
        {
            return new BaseResponse()
            {
                Message = "You not approved to be a seller in this community",
                Status = false
            };
        }

        if (request.Stock <=0)
        {
            return new BaseResponse()
            {
                Message = "Stock can not be less than or equal to 0",
                Status = false
            };
        }

        var product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            ImageUrl = request.ImageUrl,
            SellerId = seller.Id,
            CommunityId = communityId,
            CategoryId = request.CategoryId,
            Tags = request.Tags,
            Status = ProductStatus.Pending
        };
        await _productRepository.AddAsync(product);
        return new BaseResponse()
        {
            Message = "Product added",
            Status = true
        };
    }

    public async Task<ProductResponseModel> GetProductById(string productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            return new ProductResponseModel()
            {
                Status = false,
                Message = "Product not found",
            };
        }

        return new ProductResponseModel()
        {
            Status = true,
            Data = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
                CommunityId = product.CommunityId,
                Status = product.Status.ToString(),
                CommunityName = product.Community.Name,
                SellerName = product.Seller.User.Username
            },
            Message = "Product retrieved"
        };
    }

    public async Task<ProductsResponseModel> GetProductByCommunity(string communityId)
    {
        var products = await _productRepository.GetByCommunityAsync(communityId);
        return new ProductsResponseModel()
        {
            Status = true,
            Data = products.Select(p => new ProductDTO()
            {
                Id = p.Id,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                SellerId = p.SellerId,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                SellerName = p.Seller.User.Username,
                Description = p.Description,
                Name = p.Name,
                Status = p.Status.ToString()
            }).ToList(),
            Message = "Product retrieved"
        };
    }

    public async Task<ProductsResponseModel> GetProductBySeller(string sellerId)
    {
        var products = await _productRepository.GetBySellerAsync(sellerId);
        return new ProductsResponseModel()
        {
            Status = true,
            Data = products.Select(p => new ProductDTO()
            {
                Id = p.Id,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                SellerId = p.SellerId,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                SellerName = p.Seller.User.Username,
                Description = p.Description,
                Name = p.Name,
                Status = p.Status.ToString()
            }).ToList(),
            Message = "Product retrieved"
        };
    }

    public async Task<BaseResponse> UpdateProduct(string productId, ProductRequestModel request)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.ImageUrl = request.ImageUrl;
        await _productRepository.UpdateAsync(product);
        return new BaseResponse()
        {
            Message = "Product updated",
            Status = true
        };
    }

    public async Task<BaseResponse> DeleteProduct(string productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "Product not found",
            };
        }
        await _productRepository.DeleteAsync(product);
        return new BaseResponse()
        {
            Message = "Product deleted",
            Status = true
        };
    }
}