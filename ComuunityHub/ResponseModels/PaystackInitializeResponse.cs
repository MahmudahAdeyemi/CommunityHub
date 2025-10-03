namespace ComuunityHub.ResponseModels;

public record PaystackInitializeResponse : BaseResponse
{
    public PaystackInitializeData Data { get; init; }
}

public record PaystackInitializeData
{
    public string AuthorizationUrl { get; init; }
    public string AccessCode { get; init; }
    public string Reference{get;init;}
}