using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record CartItemResponseModel : BaseResponse
{
    public CartItemDTO Data { get; set; }
}