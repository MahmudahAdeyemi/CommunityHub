using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record CategoriesResponseModel : BaseResponse
{
    public List<CategoryDTO> Data { get; set; } = [];
}