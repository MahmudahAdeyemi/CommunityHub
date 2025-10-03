namespace ComuunityHub.DTOs;

public record SubOrderDTO
{
    public string SubOrderId { get; set; }
    public string SellerId { get; set; }
    public string Status { get; set; }   
    public decimal SubTotal { get; set; }
    public List<OrderItemDTO> Items { get; set; } = [];
}