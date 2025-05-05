namespace KalkulatorWalutowy.Model;

public class ExchangeRateResponse
{
    public string table { get; set; } = string.Empty;
    public string currency { get; set; } = string.Empty;
    public string code { get; set; } = string.Empty;
    public string no { get; set; } = string.Empty;
    public string effectiveDate { get; set; } = string.Empty;
    public List<Rate> rates { get; set; } = new List<Rate>();
}
