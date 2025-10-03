namespace ComuunityHub.RequestModels;

public record FezRequestModel
{
    public string RecipientAddress { get; set; }
    public string RecipientState { get; set; }
    public string RecipientName { get; set; }
    public string RecipientPhone { get; set; }
    public string RecipientEmail { get; set; }
    public string UniqueID { get; set; }
    public string BatchID { get; set; }
    public string ValueOfItem { get; set; }
    public decimal Weight { get; set; }
    public string ItemDescription { get; set; }
    public bool IsItemCod { get; set; } = false;
    public decimal? CashOnDeliveryAmount { get; set; }
    public bool Fragile { get; set; } = false;
}