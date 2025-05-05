
namespace KalkulatorWalutowy.Model;

public class Rate
{
    public string no { get; set; } = string.Empty;
    public string effectiveDate { get; set; } = string.Empty;
    public double mid { get; set; }
    public string code { get; set; } = string.Empty;
}
