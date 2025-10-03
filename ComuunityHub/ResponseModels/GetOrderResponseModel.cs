using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetOrderResponseModel : BaseResponse
{
    public OrderDTO Data { get; set; }
}