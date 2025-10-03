using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetOrdersResponseModel : BaseResponse
{
    public List<OrderDTO> Data { get; set; } = [];
}