using Newtonsoft.Json;

public class ExchangeRate
{
    [JsonProperty("result")]
    public string? Result { get; set; }

    [JsonProperty("conversion_rate")]
    public decimal ConversionRate { get; set; }
}
