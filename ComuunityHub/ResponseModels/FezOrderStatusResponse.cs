namespace ComuunityHub.ResponseModels;

public class FezOrderStatusResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public List<OrderStatusDetails> Orders { get; set; }
}
public class OrderStatusDetails
{
    public string OrderId { get; set; }
    public string Status { get; set; }
    public string CurrentLocation { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
