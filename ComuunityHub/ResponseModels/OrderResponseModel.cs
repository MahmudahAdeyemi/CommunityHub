namespace ComuunityHub.ResponseModels;

public record OrderResponseModel : BaseResponse
{
    public string OrderId { get; init; }
}