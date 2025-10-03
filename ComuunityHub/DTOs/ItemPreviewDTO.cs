namespace ComuunityHub.DTOs;

public record ItemPreviewDTO
{
    public string ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public string ProductName { get; init; }
    public string ProductImageUrl { get; init; }
}