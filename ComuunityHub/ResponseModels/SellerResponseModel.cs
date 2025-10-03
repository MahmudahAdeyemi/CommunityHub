using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record SellerResponseModel : BaseResponse
{
    public SellerDTO Data { get; init; }
}