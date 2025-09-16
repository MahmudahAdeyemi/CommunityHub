using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ComuunityHub.DTOs;

namespace ComuunityHub.Implementation.Services;

public class CommunityService : ICommunityService
{
    private readonly ICommunityRepository _communityRepository;
    private readonly ICommunityMemberRepository _communityMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommunityService(ICommunityRepository communityRepository, IUserRepository userRepository,
        IEmailService emailService, IHttpContextAccessor httpContextAccessor,ICommunityMemberRepository communityMemberRepository)
    {
        _communityRepository = communityRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _communityMemberRepository = communityMemberRepository;
    }

    public async Task<BaseResponse> CreateCommunity(CreateCommunityRequestModel model)
    {
        var check = await _communityRepository.GetCommunityByName(model.Name);
        if (check != null)
        {
            return new BaseResponse()
            {
                Message = "The community with that name already exists.",
                Status = false
            };
        }
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var user = await _userRepository.GetUserAsync(userId);
        Community community = new Community()
        {
            Name = model.Name,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            CreatedBy = user,
            Status = CommunityStatus.Pending
        };
        await _communityRepository.CreateCommunity(community);
        string subject = "Your Community Has Been Created – Pending Approval";
        string body =
            $"Hello {user.Username},\nThank you for creating a new community on CommunityHub! 🎉\nYour community {community.Name} has been successfully created and is currently pending admin approval.\nWe’ll review it shortly to ensure it meets our guidelines.\nYou’ll receive a notification once it’s approved and made visible to other users.\nIf you have any questions in the meantime, feel free to reach out to us at officialadeyemi.mahmudah@gmail.com.\nBest regards,\nThe CommunityHub Team";
        await _emailService.SendEmailAsync(user.Email, subject, body,false);
        return new BaseResponse()
        {
            Message = "Your Community has been successfully created.",
            Status = true
        };
    }

    public async Task<GetAllCommunityResponseModel> GetAllApprovedCommunity()
    {
        var communities =await _communityRepository.GetAllAsync();
        var approvedCommunities = communities.Where(c => c.Status == CommunityStatus.Approved);
        return new GetAllCommunityResponseModel()
        {
            Data = approvedCommunities.Select(c => new GetCommunityDTO
            {
                Name = c.Name,
                Description = c.Description,
                Status = c.Status.ToString(),
                NoOfMembers = c.Members.Count,
                Created = c.CreatedAt,
                Members = c.Members
            }).ToList(),
            Status = true,
            Message = "Successfully retrieved all approved communities.",
        };
    }
    public async Task<GetAllCommunityResponseModel> GetAllPendingCommunity()
    {
        var communities =await _communityRepository.GetAllAsync();
        var approvedCommunities = communities.Where(c => c.Status == CommunityStatus.Pending);
        return new GetAllCommunityResponseModel()
        {
            Data = approvedCommunities.Select(c => new GetCommunityDTO
            {
                Name = c.Name,
                Description = c.Description,
                Status = c.Status.ToString(),
                NoOfMembers = c.Members.Count,
                Created = c.CreatedAt,
                Members = c.Members
            }).ToList(),
            Status = true,
            Message = "Successfully retrieved all approved communities.",
        };
    }

    public async Task<BaseResponse> ApproveCommunity(string communityId)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        if (community == null)
        {
            return new BaseResponse()
            {
                Message = "The community with that ID does not exist.",
                Status = false
            };
        }

        if (community.Status != CommunityStatus.Pending)
        {
            return new BaseResponse()
            {
                Message = "The community has been either approved or rejected.",
                Status = false
            };
        }
        community.Status = CommunityStatus.Approved;
        await _communityRepository.UpdateCommunity(community);
        CommunityMember member = new CommunityMember
        {
            UserId = community.CreatedBy.Id,
            User = community.CreatedBy,
            Status = CommunityStatus.Approved,
            CommunityId = community.Id,
            Role = community.CreatedBy.Roles,
            CommunityRole = CommunityRole.Creator,
            JoinedAt = community.CreatedAt
        };
        await _communityMemberRepository.AddCommunityMember(member);
        
        string subject = "Your Community Has Been Approved 🎉";
        string body =
            $"Hello <b>{community.CreatedBy.Username}</b>, <br><br>\n\nGood news! Your community <b>{community.Name}</b> has been <span style=\"color:green;\">approved</span> by our admin team. 🎊<br><br>\n\nYour community is now live and visible to other users. You can start engaging members and building conversations right away.<br><br>\n\n<a href=\"[CommunityLink]\">👉 Click here to visit your community</a><br><br>\n\nBest regards,<br>\nThe <b>CommunityHub</b> Team";
        await _emailService.SendEmailAsync(community.CreatedBy.Email, subject, body, true);
        return new BaseResponse()
        {
            Message = "The community has been approved.",
            Status = true
        };
    }

    public async Task<BaseResponse> RejectCommunity(string communityId)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        if (community == null)
        {
            return new BaseResponse()
            {
                Message = "The community with that ID does not exist.",
                Status = false
            };
        }
        if (community.Status != CommunityStatus.Pending)
        {
            return new BaseResponse()
            {
                Message = "The community has been approved or rejected.",
                Status = false
            };
        }
        community.Status = CommunityStatus.Rejected;
        await _communityRepository.UpdateCommunity(community);
        string subject = "Your Community Request Has Been Rejected";
        string body =
            $"Hello <b>{community.CreatedBy.Username}</b>, <br><br>\n\nWe appreciate your effort in creating the community <b>{community.Name}</b> on <b>CommunityHub</b>. <br><br>\n\nUnfortunately, after review, your community has been <span style=\"color:red;\">rejected</span> by our admin team. This may be due to it not meeting our community guidelines or requirements. <br><br>\n\nIf you’d like to try again, please review our guidelines and submit a new community request.<br><br>\n\nFor more details, feel free to reach out to us at <a href=\"mailto:officialadeyemi.mahmudah@gmail.com\">[SupportEmail]</a>. <br><br>\n\nBest regards,<br>\nThe <b>CommunityHub</b> Team";
        await _emailService.SendEmailAsync(community.CreatedBy.Email, subject, body, true);
        return new BaseResponse()
        {
            Message = "The community has been rejected.",
            Status = true
        };
    }
    
    public async Task<GetCommunityResponseModel> GetCommunityById(string id)
    {
        var community = await _communityRepository.GetByIdAsync(id);
        return new GetCommunityResponseModel
        {
            Data = new GetCommunityDTO
            {
                Name = community.Name,
                Description = community.Description,
                Status = community.Status.ToString(),
                NoOfMembers = community.Members.Count,
                Created = community.CreatedAt,
                Members = community.Members
            },
            Status = true,
            Message = "Successfully retrieved community.",
        };
    }

    public async Task<GetAllCommunityResponseModel> GetUserCommunities()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var communities = await _communityRepository.GetUserCommunity(userId);
        return new GetAllCommunityResponseModel
        {
            Data = communities.Select(x => new GetCommunityDTO
            {
                Name = x.Name,
                Description = x.Description,
                Status = x.Status.ToString(),
                NoOfMembers = x.Members.Count,
                Created = x.CreatedAt,
                Members = x.Members
            }).ToList(),
            Status = true,
            Message = "Successfully retrieved all user communities.",
        };
    }

    public async Task<BaseResponse> UpdateCommunity(string communityId, UpdateCommunityRequestModel model)
    {
        var community = await _communityRepository.GetByIdAsync(communityId);
        if (community == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Community not found."
            };
        }
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (community.CreatedBy.Id != userId)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "You are not authorized to update this community."
            };
        }
        community.Name = model.Name;
        community.Description = model.Description;

        await _communityRepository.UpdateCommunity(community);

        return new BaseResponse
        {
            Status = true,
            Message = "Community updated successfully."
        };
    }
}
