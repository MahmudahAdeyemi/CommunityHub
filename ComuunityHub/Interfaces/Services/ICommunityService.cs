using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public interface ICommunityService
{
    Task<BaseResponse> CreateCommunity(CreateCommunityRequestModel model);
    Task<GetAllCommunityResponseModel> GetAllApprovedCommunity();
    Task<GetAllCommunityResponseModel> GetAllPendingCommunity();
    Task<GetCommunityResponseModel> GetCommunityById(string id);
    Task<GetAllCommunityResponseModel> GetUserCommunities();
    Task<BaseResponse> ApproveCommunity(string communityId);
    Task<BaseResponse> RejectCommunity(string communityId);
    Task<BaseResponse> UpdateCommunity(string communityId, UpdateCommunityRequestModel model);
}