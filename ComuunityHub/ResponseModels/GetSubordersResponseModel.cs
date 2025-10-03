using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetSubordersResponseModel : BaseResponse
{
    public List<SubOrderDTO> Data { get; set; } = [];
}