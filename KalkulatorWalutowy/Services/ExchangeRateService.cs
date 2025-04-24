using System.Text.Json;
using Newtonsoft.Json;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient = new();

    public async Task<decimal> GetCryptoRateAsync(string crypto, string fiat)
    {
        using var httpClient = new HttpClient();
        var url = $"https://api.coingecko.com/api/v3/simple/price?ids={crypto}&vs_currencies={fiat}";
        var response = await httpClient.GetStringAsync(url);

        var json = JsonDocument.Parse(response);
        var rate = json.RootElement.GetProperty(crypto).GetProperty(fiat).GetDecimal();
        return rate;
    }


    public async Task<decimal> GetFiatRateAsync(string from, string to)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync($"https://api.exchangerate.host/convert?from={from}&to={to}");

        var json = JsonDocument.Parse(response);
        var rate = json.RootElement.GetProperty("result").GetDecimal();
        return rate;
    }

}
