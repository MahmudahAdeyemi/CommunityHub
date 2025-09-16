using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetCommunityResponseModel : BaseResponse
{
    public GetCommunityDTO Data { get; init; }
}