using ComuunityHub.Implementation.Services;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;

public class CommunityController : ControllerBase
{
    private readonly ICommunityMemberService _communityMemberService;
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityMemberService communityMemberService, ICommunityService communityService)
    {
        _communityMemberService = communityMemberService;
        _communityService = communityService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityRequestModel model)
    {
        var response = await _communityService.CreateCommunity(model);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCommunities()
    {
        var response = await _communityService.GetAllApprovedCommunity();

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommunityById(string id)
    {
        var response = await _communityService.GetCommunityById(id);

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingCommunities()
    {
        var response = await _communityService.GetAllPendingCommunity();

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveCommunity(string id)
    {
        var response = await _communityService.ApproveCommunity(id);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectCommunity(string id)
    {
        var response = await _communityService.RejectCommunity(id);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpPut("{communityId}")]
    public async Task<IActionResult> UpdateCommunity(string communityId,  [FromBody] UpdateCommunityRequestModel model)
    {
        var response = await _communityService.UpdateCommunity(communityId,model);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
}