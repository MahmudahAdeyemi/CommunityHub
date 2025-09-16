using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record GetAllMembersResponseModel : BaseResponse
{
    public List<MemberDTO> Data { get; set; } = [];
}