namespace ComuunityHub.ResponseModels;

public record FezResponseModel
{
    public string CurrentStatus { get; set; }
    public DateTime LastUpdated { get; set; }
}