using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record CategoryResponseModel : BaseResponse
{
    public CategoryDTO Data { get; set; }
}