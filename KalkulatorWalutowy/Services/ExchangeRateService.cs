using System.Text.Json;
using System.Text.Json.Serialization;
using KalkulatorWalutowy.Model;

namespace KalkulatorWalutowy.Services;

public class ExchangeRateService
{
    private static readonly HttpClient client = new();

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public async Task<double> GetExchangeRateAsync(string currencyCode)
    {
        if (currencyCode == "PLN")
            return 1.0;

        string url = $"https://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, _jsonOptions);

            return data?.Rates?.FirstOrDefault()?.Mid
                   ?? throw new Exception("No rate data");
        }
        else
        {
            throw new Exception($"Error while downloading data from {currencyCode}: {response.StatusCode}");
        }
    }

    public async Task<List<string>> GetAvailableCurrenciesAsync()
    {
        string url = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var tables = JsonSerializer.Deserialize<List<ExchangeRateResponse>>(json, _jsonOptions);

            var currencies = tables?.FirstOrDefault()?.Rates?.Select(rate => rate.Code).ToList()
                            ?? new List<string>();

            if (!currencies.Contains("PLN"))
                currencies.Insert(0, "PLN");

            return currencies;
        }
        else
        {
            throw new Exception("Couldn't generate currencies list");
        }
    }
}
