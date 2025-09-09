namespace ComuunityHub.ResponseModels;

public record RegisterUserResponseModel : BaseResponse
{
    public string UserId { get; set; }
}