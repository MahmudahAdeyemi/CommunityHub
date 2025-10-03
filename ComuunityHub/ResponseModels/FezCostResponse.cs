namespace ComuunityHub.ResponseModels;

public class FezCostResponse
{
    public string Status { get; set; }
    public string Description { get; set; }
    public List<CostDetails> Cost { get; set; }
}
public class CostDetails
{
    public decimal cost { get; set; }
    public string service { get; set; }
}