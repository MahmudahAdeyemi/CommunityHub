using ComuunityHub.DTOs;

namespace ComuunityHub.ResponseModels;

public record CheckoutPreviewResponseModel : BaseResponse
{
    public decimal Subtotal { get; init; }
    public decimal Total { get; init; }
    public decimal ShippingFee { get; init; }
    public List<ItemPreviewDTO> Items { get; init; } = [];
}