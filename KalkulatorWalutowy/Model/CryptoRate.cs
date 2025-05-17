namespace KalkulatorWalutowy.Model;

public class CryptoRate
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Is_Active { get; set; }
}
