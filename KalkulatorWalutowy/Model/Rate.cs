
namespace KalkulatorWalutowy.Model;

public class Rate
{
    public string No { get; set; } = string.Empty;
    public string EffectiveDate { get; set; } = string.Empty;
    public double Mid { get; set; }
    public string Code { get; set; } = string.Empty;
}
