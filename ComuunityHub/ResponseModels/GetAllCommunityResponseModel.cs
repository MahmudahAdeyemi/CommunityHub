using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetAllCommunityResponseModel : BaseResponse
{
    public List<GetCommunityDTO> Data { get; init; } = [];
}