using ComuunityHub.DTOs;
using ComuunityHub.RequestModels;

namespace ComuunityHub.Interfaces.Services;

public interface IShippingService
{
    decimal CalculateShippingFeeAsync(string sellerId, AddressRequestModel DeliveryAddress,
        List<ItemPreviewDTO> Items);
}