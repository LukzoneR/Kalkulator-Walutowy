
namespace KalkulatorWalutowy.Model;

public class ExchangeRateResponse
{
    public string? table { get; set; }
    public string? currency { get; set; }
    public string? code { get; set; }
    public List<Rate> rates { get; set; } = new List<Rate>();
}
