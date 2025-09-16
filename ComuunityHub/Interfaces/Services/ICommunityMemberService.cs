using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface ICommunityMemberService
{
    Task<BaseResponse> JoinCommunity(string communityId);
    Task<BaseResponse> LeaveCommunity(string communityId);
    Task<GetAllMembersResponseModel> GetAllMembers(string communityId);
    Task<BaseResponse> ApproveMember(string communityId, string userId);
    Task<BaseResponse> RejectMember(string communityId, string userId);
    Task<BaseResponse> RemoveMemberAsync(string communityId, string userId);
}