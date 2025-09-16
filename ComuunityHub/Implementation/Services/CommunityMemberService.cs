using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ComuunityHub.DTOs;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public class CommunityMemberService : ICommunityMemberService
{
    private readonly ICommunityMemberRepository _communityMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommunityRepository _communityRepository;
    private readonly IEmailService _emailService;
    public CommunityMemberService(ICommunityMemberRepository communityMemberRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ICommunityRepository communityRepository, IEmailService emailService)
    {
        _communityMemberRepository = communityMemberRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _communityRepository = communityRepository;
        _emailService = emailService;
    }

    public async Task<BaseResponse> JoinCommunity(string communityId)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var user = await _userRepository.GetUserAsync(userId);
        var community = await _communityRepository.GetByIdAsync(communityId);
        if (community == null || community.Status != CommunityStatus.Approved)
        {
            return new BaseResponse
            {
                Message = "Community doesn't exist",
                Status = false
            };
        }
        var membership = await _communityMemberRepository.GetMembershipAsync(communityId, userId);
        if (membership != null)
        {
            return new BaseResponse
            {
                Message = "You are already joined the community",
                Status = false
            };
        }

        var member = new CommunityMember()
        {
            JoinedAt = DateTime.Now,
            Community = community,
            User = user,
            CommunityId = community.Id,
            Status = CommunityStatus.Pending,
            UserId = userId,
            Role = user.Roles,
            CommunityRole = CommunityRole.Member
        };
        await _communityMemberRepository.AddCommunityMember(member);
        string subject = $"Your Request to Join {community.Name} is Pending Approval";
        string body =
            $"Hello {user.Username},\nWe’ve received your request to join {community.Name} on CommunityHub. ✅\nYour request has been submitted successfully and is now pending approval from the community admin.\nYou’ll be notified as soon as your request is approved (or declined).\nThank you for being part of our growing community! 🌟\nBest regards,\nThe CommunityHub Team";
        
        await _emailService.SendEmailAsync(user.Email, subject, body,false);
        subject = "New Join Request for Your Community";
        body =
            $"Hello {community.CreatedBy.Username},\nGood news! 🎉\nA user has requested to join your community {community.Name} on CommunityHub.\nHere are the details:\nRequester: {user.Username}\nCommunity: {community.Name}\nThe request is currently pending your approval. Please log in to your dashboard to review and either approve or decline the request.\nThank you for helping us keep communities safe and engaging!\nBest regards,\nThe CommunityHub Team";
        await _emailService.SendEmailAsync(community.CreatedBy.Email, subject, body, false);
        return new BaseResponse()
        {
            Message = "Your request has been submitted successfully",
            Status = true
        };
    }

    public async Task<BaseResponse> LeaveCommunity(string communityId)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var membership = await _communityMemberRepository.GetMembershipAsync(communityId, userId);
        if (membership == null)
        {
            return new BaseResponse
            {
                Message = "You are not a member of the community",
                Status = false
            };
        }

        await _communityMemberRepository.RemoveMemberAsync(membership);
        return new BaseResponse()
        {
            Message = "You have left the community successfully",
            Status = false
        };
    }

    public async Task<GetAllMembersResponseModel> GetAllMembers(string communityId)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        var members = community.Members.ToList();
        if (members.Count == 0)
        {
            return new GetAllMembersResponseModel
            {
                Message = "There are no members of the community",
                Status = false
            };
        }

        return new GetAllMembersResponseModel()
        {
            Data = members.Select(x => new MemberDTO
            {
                Username = x.User.Username,
                Email = x.User.Email,
                JoinedAt = x.JoinedAt
            }).ToList(),
            Message = "Successfully retrieved all members",
            Status = true
        };
    }

    public async Task<BaseResponse> ApproveMember(string communityId, string userId)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        var user = await _userRepository.GetUserAsync(userId);
        var membership = await _communityMemberRepository.GetMembershipAsync(communityId, userId);
        if (membership == null)
        {
            return new BaseResponse
            {
                Message = "User has not joined the community",
                Status = false
            };
        }

        if (membership.Status != CommunityStatus.Pending)
        {
            return new BaseResponse
            {
                Message = "User has either been approved or rejected",
                Status = false
            };
        }
        membership.Status = CommunityStatus.Approved;
        await _communityMemberRepository.UpdateMemberAsync(membership);
        string subject = $"Your Request to Join {community.Name} Has Been Approved 🎉";
        string body =
            $"Hello <b{user.Username}</b>, <br><br>\n\nGreat news! 🎊 Your request to join the community <b>{community.Name}</b> has been <span style=\"color:green;\">approved</span>. <br><br>\n\nYou can now connect with other members, share ideas, and be part of the discussions. <br><br>\n\n<a href=\"[CommunityLink]\">👉 Click here to visit the community</a><br><br>\n\nWe’re excited to have you onboard! <br><br>\n\nBest regards,<br>\nThe <b>CommunityHub</b> Team";
        await _emailService.SendEmailAsync(user.Email, subject, body, true);
        return new BaseResponse
        {
            Message = "User has been approved",
            Status = true
        };
        
    }
    
    public async Task<BaseResponse> RejectMember(string communityId, string userId)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        var user = await _userRepository.GetUserAsync(userId);
        var membership = await _communityMemberRepository.GetMembershipAsync(communityId, userId);
        if (membership == null)
        {
            return new BaseResponse
            {
                Message = "User has not joined the community",
                Status = false
            };
        }

        if (membership.Status != CommunityStatus.Pending)
        {
            return new BaseResponse
            {
                Message = "User has either been approved or rejected",
                Status = false
            };
        }
        membership.Status = CommunityStatus.Rejected;
        await _communityMemberRepository.UpdateMemberAsync(membership);
        string subject = $"Your Request to Join {community.Name} Has Been Rejected";
        string body =
            $"Hello <b>{user.Username}</b>, <br><br>\n\nThank you for your interest in joining the community <b>{community.Name}</b> on <b>CommunityHub</b>. <br><br>\n\nUnfortunately, your request has been <span style=\"color:red;\">rejected</span> by the community creator at this time. <br><br>\n\nYou’re welcome to explore and join other amazing communities on <b>CommunityHub</b> that may be a better fit for you. <br><br>\n\n<a href=\"[CommunitiesBrowseLink]\">👉 Browse other communities</a><br><br>\n\nBest regards,<br>\nThe <b>CommunityHub</b> Team";
            await _emailService.SendEmailAsync(user.Email, subject, body, true);
        return new BaseResponse
        {
            Message = "User has been rejected",
            Status = true
        };
        
    }
    
    public async Task<BaseResponse> RemoveMemberAsync(string communityId, string userId)
    {
        var member = await _communityMemberRepository.GetMembershipAsync(communityId, userId);
        if (member == null || member.Status != CommunityStatus.Approved)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "User is not an active member of this community."
            };
        }

        await _communityMemberRepository.RemoveMemberAsync(member);

        return new BaseResponse
        {
            Status = true,
            Message = "Member removed successfully."
        };
    }
    
}