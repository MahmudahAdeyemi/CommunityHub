namespace ComuunityHub.ResponseModels;

public class FezTrackOrderResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public TrackingDetails TrackingDetails { get; set; }
}

public class TrackingDetails
{
    public string OrderId { get; set; }
    public string Status { get; set; }
    public string CurrentLocation { get; set; }
    public DateTime EstimatedDelivery { get; set; }
}