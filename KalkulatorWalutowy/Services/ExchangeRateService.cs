using System.Text.Json;
using KalkulatorWalutowy.Model;

namespace KalkulatorWalutowy.Services;

public class ExchangeRateService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<double> GetExchangeRateAsync(string currencyCode)
    {
        if (currencyCode == "PLN")
            return 1.0;

        string url = $"https://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json);
            return data?.rates?.FirstOrDefault()?.mid
                   ?? throw new Exception("Brak danych kursowych.");
        }
        else
        {
            throw new Exception($"Błąd podczas pobierania kursu dla {currencyCode}: {response.StatusCode}");
        }
    }

    public async Task<List<string>> GetAvailableCurrenciesAsync()
    {
        string url = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var tables = JsonSerializer.Deserialize<List<ExchangeRateResponse>>(json);
            var currencies = tables?.FirstOrDefault()?.rates?.Select(rate => rate.code).ToList() ?? new List<string>();

            if (!currencies.Contains("PLN"))
                currencies.Insert(0, "PLN");

            return currencies;
        }
        else
        {
            throw new Exception("Nie udało się pobrać listy walut.");
        }
    }
}