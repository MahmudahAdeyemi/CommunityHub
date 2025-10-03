namespace ComuunityHub.DTOs;

public record CartSummaryDTO
{
    public string CartId { get; set; }
    public List<CartItemDTO> CartItems { get; set; } = [];
    public decimal Total { get; set; }
}