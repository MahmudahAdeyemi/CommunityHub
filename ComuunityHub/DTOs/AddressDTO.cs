namespace ComuunityHub.DTOs;

public record AddressDTO
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}