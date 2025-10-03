using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.ResponseModels;
using Microsoft.Extensions.Options;

namespace ComuunityHub.Implementation.Services;

public class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly string _secretKey;
    public PaystackService(IOptions<PaystackSettings> options)
    {
        _httpClient = new HttpClient();
        _secretKey = options.Value.SecretKey;
    }

    public async Task<PaystackInitializeResponse> InitializePayment(string email, decimal amount, string orderId)
    {
        var requestBody = new
        {
            email = email,
            amount = (int)(amount * 100),
            callback_url = "http://localhost:5000/api/payment/verify",
            metadata = new
            {
                project = "ComuunityHub",
                orderId = orderId
            }
        };
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(requestBody, options);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<PaystackInitializeResponse>(responseContent, options);
        return result;
    }

    public async Task<PayStackVerifyResponse> VerifyPayment(string reference)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<PayStackVerifyResponse>(responseContent, options);
        return result;
    }
}