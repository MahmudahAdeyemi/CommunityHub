using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record ProductsResponseModel : BaseResponse
{
    public List<ProductDTO> Data { get; set; } = [];
}