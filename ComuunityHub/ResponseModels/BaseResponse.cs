namespace ComuunityHub.ResponseModels;

public record BaseResponse
{
    public string Message { get; set; }
    public bool Status { get; set; }
}