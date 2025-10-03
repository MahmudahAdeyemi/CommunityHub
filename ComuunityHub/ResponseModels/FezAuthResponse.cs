namespace ComuunityHub.ResponseModels;

public class FezAuthResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public AuthDetails AuthDetails { get; set; }
}

public class AuthDetails
{
    public string AuthToken { get; set; }
    public DateTime ExpireToken { get; set; }
}