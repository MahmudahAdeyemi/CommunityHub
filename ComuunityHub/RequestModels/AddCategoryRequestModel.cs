namespace ComuunityHub.RequestModels;

public record AddCategoryRequestModel
{
    public string Name { get; set; }
    public string? ParentCategoryId { get; set; }
}