using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SellerController : Controller
{
    private readonly ISellerService _sellerService;

    public SellerController(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterSeller([FromQuery] string communityId, [FromBody]RegisterSellerRequestModel request)
    {
        var response = await _sellerService.RegisterSeller(communityId, request);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
    [HttpPost("{sellerId}/approve")]
    public async Task<IActionResult> ApproveSeller(string sellerId)
    {
        var response = await _sellerService.ApproveSellerAsync(sellerId);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
    [HttpPost("{sellerId}/reject")]
    public async Task<IActionResult> RejectSeller(string sellerId)
    {
        var response = await _sellerService.RejectSellerAsync(sellerId);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("{sellerId}")]
    public async Task<IActionResult> GetSellerById(string sellerId)
    {
        var response = await _sellerService.GetSellerById(sellerId);
        if (response.Status) return Ok(response);
        return NotFound(response);
    }
    [HttpGet("by-community/{communityId}")]
    public async Task<IActionResult> GetSellersByCommunity(string communityId)
    {
        var response = await _sellerService.GetSellersByCommunityAsync(communityId);
        if (response.Status) return Ok(response);
        return NotFound(response);
    }
    [HttpDelete("{sellerId}")]
    public async Task<IActionResult> DeleteSeller(string sellerId)
    {
        var response = await _sellerService.DeleteSellerAsync(sellerId);
        if (response.Status) return Ok(response);
        return BadRequest(response);
    }
}