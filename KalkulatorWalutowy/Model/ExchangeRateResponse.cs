namespace KalkulatorWalutowy.Model;

public class ExchangeRateResponse
{
    public string Table { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string No { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public List<Rate> Rates { get; set; } = new List<Rate>();
}
