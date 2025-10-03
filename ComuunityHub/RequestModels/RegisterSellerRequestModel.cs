namespace ComuunityHub.RequestModels;

public record RegisterSellerRequestModel
{
    public string StoreName { get; init; }
    public AddressRequestModel Address { get; init; }
}