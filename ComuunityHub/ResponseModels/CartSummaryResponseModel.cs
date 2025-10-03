using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record CartSummaryResponseModel : BaseResponse
{
    public CartSummaryDTO Data { get; init; }
}