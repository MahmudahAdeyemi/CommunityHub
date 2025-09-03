namespace ComuunityHub.Models;

public class Payment
{
    public string Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public Order Order { get; set; }
}