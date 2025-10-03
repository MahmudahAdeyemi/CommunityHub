using System.Text.Json;
using ComuunityHub.DTOs;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;
using Microsoft.Extensions.Options;

namespace ComuunityHub.Implementation.Services;

public class ShippingService : IShippingService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _secretKey;
    private readonly string _userId;
    private readonly string _password;
    private string _authToken;

    public ShippingService(IOptions<FezDelivery> options)
    {
        _httpClient = new HttpClient();
        _baseUrl = options.Value.BaseUrl;
        _secretKey = options.Value.SecretKey;
        _userId = options.Value.UserId;
        _password = options.Value.Password;
    }
    public decimal CalculateShippingFeeAsync(string sellerId, AddressRequestModel DeliveryAddress,
        List<ItemPreviewDTO> Items)
    {
        var totalItems = Items.Sum(x => x.Quantity);
        decimal fee = 500 + (200 * totalItems);
        return fee;
    }
    
    public async Task<string> AuthenticateAsync()
    {
        var url = $"{_baseUrl}/user/authenticate";
        var payload = new
        {
            user_id = _userId,
            password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(url, payload);

        
        var authResponse = await response.Content.ReadFromJsonAsync<FezAuthResponse>();

        if (authResponse != null && authResponse.Status == "Success")
        {
            _authToken = authResponse.AuthDetails.AuthToken;
            return _authToken;
        }

        throw new Exception("Fez Authentication Failed: " + authResponse?.Description);
    }
    
    public async Task<decimal> CalculateDeliveryCostAsync(string customerState, string sellerState, int numberOfItems)
    {
        await AddAuthHeaders();

        var url = $"{_baseUrl}/order/cost";
        var payload = new
        {
            state = customerState,
            pickUpState = sellerState,
            weight = (decimal)numberOfItems,
            locker = false
        };

        var response = await _httpClient.PostAsJsonAsync(url, payload);
        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var costResponse = JsonSerializer.Deserialize<FezCostResponse>(responseContent, options);

        if (costResponse != null && costResponse.Status == "Success")
        {
            return costResponse.Cost.FirstOrDefault()?.cost ?? 0m;
        }

        throw new Exception("Failed to fetch delivery cost: " + costResponse?.Description);
    }
    
    
    private async Task AddAuthHeaders()
    {
        await AuthenticateAsync();
        
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
        _httpClient.DefaultRequestHeaders.Add("secret-key", _secretKey);
    }
    public async Task<string> CreateOrderAsync(FezRequestModel order)
    {
        await AddAuthHeaders();

        var url = $"{_baseUrl}/order";
    
        var payload = new[] { order };

        var response = await _httpClient.PostAsJsonAsync(url, payload);
        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var orderResponse = JsonSerializer.Deserialize<FezOrderResponse>(responseContent, options);

        if (orderResponse != null && orderResponse.Status == "Success")
        {
            return "Order Created Successfully";
        }

        if (orderResponse?.DuplicateUniqueIds != null && orderResponse.DuplicateUniqueIds.Any())
        {
            var duplicates = string.Join(", ", orderResponse.DuplicateUniqueIds.Keys);
            throw new Exception($"Duplicate unique ID(s): {duplicates}");
        }

        throw new Exception("Failed to create order: " + orderResponse?.Description);
    }
    
    public async Task<TrackingDetails> TrackOrderAsync(string orderId)
    {
        await AddAuthHeaders();

        var url = $"{_baseUrl}/order/track/{orderId}";

        var response = await _httpClient.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var trackResponse = JsonSerializer.Deserialize<FezTrackOrderResponse>(responseContent, options);

        if (trackResponse != null && trackResponse.Status == "Success")
        {
            return trackResponse.TrackingDetails;
        }

        throw new Exception("Failed to track order: " + trackResponse?.Description);
    }
    
    public async Task<List<OrderStatusDetails>> GetOrdersStatusByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        await AddAuthHeaders(); 

        var url = $"{_baseUrl}/order/status/date-range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        var ordersStatus = JsonSerializer.Deserialize<FezOrderStatusResponse>(responseContent, options);

        if (ordersStatus != null && ordersStatus.Status == "Success")
        {
            return ordersStatus.Orders ?? new List<OrderStatusDetails>();
        }

        throw new Exception("Failed to fetch orders: " + ordersStatus?.Description);
    }
}