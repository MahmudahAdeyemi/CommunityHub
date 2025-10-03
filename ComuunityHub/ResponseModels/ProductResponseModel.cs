using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record ProductResponseModel : BaseResponse
{
    public ProductDTO Data { get; set; }
}