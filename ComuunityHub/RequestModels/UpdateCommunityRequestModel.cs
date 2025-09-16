namespace ComuunityHub.RequestModels;

public record UpdateCommunityRequestModel
{
    public string Name { get; init; }
    public string Description { get; init; }
}