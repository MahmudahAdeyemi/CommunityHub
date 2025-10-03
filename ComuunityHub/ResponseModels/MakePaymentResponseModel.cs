namespace ComuunityHub.ResponseModels;

public record MakePaymentResponseModel : BaseResponse
{
    public string AuthorizationUrl { get; init; }
}