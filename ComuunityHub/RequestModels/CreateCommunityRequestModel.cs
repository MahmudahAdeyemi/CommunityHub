namespace ComuunityHub.RequestModels;

public record CreateCommunityRequestModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}