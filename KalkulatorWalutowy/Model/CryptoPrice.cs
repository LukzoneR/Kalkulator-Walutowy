using Newtonsoft.Json;

public class CryptoPrice
{
    [JsonProperty("pln")]
    public decimal PLN { get; set; }

    [JsonProperty("eur")]
    public decimal EUR { get; set; }

    [JsonProperty("usd")]
    public decimal USD { get; set; }
}
