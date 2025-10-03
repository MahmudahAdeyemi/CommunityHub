using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : Controller
{
    private readonly IProductService  _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddProduct( [FromQuery] string communityId,[FromBody] ProductRequestModel request)
    {
        var response = await _productService.AddProduct(request,communityId);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(string productId)
    {
        var response = await _productService.GetProductById(productId);
        if (response.Status) return Ok(response);
        return NotFound(response);
    }
    [HttpGet("by-community/{communityId}")]
    public async Task<IActionResult> GetProductsByCommunity(string communityId)
    {
        var response = await _productService.GetProductByCommunity(communityId);
        if (response.Status) return Ok(response);
        return NotFound(response);
    }
    [HttpGet("by-seller/{sellerId}")]
    public async Task<IActionResult> GetProductsBySeller(string sellerId)
    {
        var response = await _productService.GetProductBySeller(sellerId);
        if (response.Status) return Ok(response);
        return NotFound(response);
    }
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(string productId,[FromBody] ProductRequestModel request)
    {
        var response = await _productService.UpdateProduct(productId, request);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {
        var response = await _productService.DeleteProduct(productId);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
}