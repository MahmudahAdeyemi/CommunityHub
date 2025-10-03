using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record SellersResponseModel :   BaseResponse
{
    public List<SellerDTO> Data { get; init; } = [];
}