namespace KalkulatorWalutowy.Model;

public class CryptoRate
{
    public string id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string symbol { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public bool is_active { get; set; }
}
