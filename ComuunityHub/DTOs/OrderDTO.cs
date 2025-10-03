using ComuunityHub.RequestModels;

namespace ComuunityHub.DTOs;

public record OrderDTO
{
    public string OrderId { get; set; }
    public string BuyerId { get; set; }
    public string BuyerName{ get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }   
    public AddressDTO DeliveryAddress { get; set; }
    public List<SubOrderDTO> SubOrders { get; set; } = new();
}
