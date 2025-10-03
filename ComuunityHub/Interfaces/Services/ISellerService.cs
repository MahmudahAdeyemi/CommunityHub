using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface ISellerService
{
    Task<BaseResponse> RegisterSeller(string communityId,RegisterSellerRequestModel request);
    Task<SellerResponseModel> GetSellerById(string sellerId);
    Task<SellersResponseModel> GetSellersByCommunityAsync(string communityId);
    Task<BaseResponse> DeleteSellerAsync(string sellerId);
    Task<BaseResponse> ApproveSellerAsync(string sellerId);
    Task<BaseResponse> RejectSellerAsync(string sellerId);
}