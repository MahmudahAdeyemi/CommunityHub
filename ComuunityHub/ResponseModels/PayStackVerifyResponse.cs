namespace ComuunityHub.ResponseModels;

public record PayStackVerifyResponse : BaseResponse
{
    public PayStackVerifyData Data { get; init; }
}

public record PayStackVerifyData
{
    public string Status { get; init; }
    public string Reference { get; init; }
    public string GatewayResponse { get; init; }
    public string PaidAt { get; init; }
    public string CreatedAt { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
}