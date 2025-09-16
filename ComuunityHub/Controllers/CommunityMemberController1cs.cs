using ComuunityHub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;

public class CommunityMemberController1cs : ControllerBase
{
    private readonly ICommunityMemberService _communityMemberService;

    public CommunityMemberController1cs(ICommunityMemberService communityMemberService)
    {
        _communityMemberService = communityMemberService;
    }
    [HttpPost("{communityId}/join")]
    public async Task<IActionResult> RequestToJoin(string communityId)
    {
        var response = await _communityMemberService.JoinCommunity(communityId);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpPost("{communityId}/approve-member")]
    public async Task<IActionResult> ApproveMember(string communityId, [FromQuery] string userId)
    {
        var response = await _communityMemberService.ApproveMember(communityId, userId);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpPost("{communityId}/reject-member")]
    public async Task<IActionResult> RejectMember(string communityId, [FromQuery] string userId)
    {
        var response = await _communityMemberService.RejectMember(communityId, userId);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpGet("{communityId}/members")]
    public async Task<IActionResult> GetMembers(string communityId)
    {
        var response = await _communityMemberService.GetAllMembers(communityId);

        if (response.Status)
            return Ok(response);

        return NotFound(response);
    }
    [HttpDelete("{communityId}/leave")]
    public async Task<IActionResult> LeaveCommunity(string communityId)
    {
        var response = await _communityMemberService.LeaveCommunity(communityId);

        if (response.Status)
            return Ok(response);

        return BadRequest(response);
    }
    [HttpDelete("{communityId}/remove-member")]
    public async Task<IActionResult> RemoveMember(string communityId, [FromQuery] string userId)
    {
        var response = await _communityMemberService.RemoveMemberAsync(communityId, userId);

        if (response.Status)    
            return Ok(response);

        return BadRequest(response);
    }
}