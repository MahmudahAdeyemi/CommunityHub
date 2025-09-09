namespace ComuunityHub.RequestModels;

public record VerifyOtpRequestModel
{
    public string UserId { get; set; }
    public string Otp{get;set;}
}