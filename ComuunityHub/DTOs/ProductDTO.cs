using ComuunityHub.Models;

namespace ComuunityHub.DTOs;

public record ProductDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Status { get; set; }
    public decimal Stock { get; set; }
    public string SellerId { get; set; }
    public string SellerName { get; set; }
    public string CommunityId { get; set; }
    public string CommunityName { get; set; }
}