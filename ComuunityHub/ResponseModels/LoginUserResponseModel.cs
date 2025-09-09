namespace ComuunityHub.ResponseModels;

public record LoginUserResponseModel : BaseResponse
{
    public bool isEmailConfirmed{get;set;}
    public string Token{get;set;}
}