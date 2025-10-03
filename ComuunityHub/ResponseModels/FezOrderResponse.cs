namespace ComuunityHub.ResponseModels;

public class FezOrderResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public Dictionary<string, string>? OrderNos { get; set; }
    public Dictionary<string, string>? DuplicateUniqueIds { get; set; }
}