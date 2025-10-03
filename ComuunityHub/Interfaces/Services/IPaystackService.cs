using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface IPaystackService
{
    Task<PaystackInitializeResponse> InitializePayment(string email, decimal amount, string orderId);
    Task<PayStackVerifyResponse> VerifyPayment(string reference);
}