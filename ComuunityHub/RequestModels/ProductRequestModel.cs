namespace ComuunityHub.RequestModels;

public record ProductRequestModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public string Tags { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public string ImageUrl { get; set; }
}