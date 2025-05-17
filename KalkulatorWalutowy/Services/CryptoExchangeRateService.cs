using System.Text.Json;
using System.Text.Json.Serialization;
using KalkulatorWalutowy.Model;

namespace KalkulatorWalutowy.Services;

public class CryptoExchangeRateService
{
    private readonly HttpClient _client = new();

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    private readonly Dictionary<string, string> cryptoIds = new()
    {
        { "BTC", "btc-bitcoin" },
        { "ETH", "eth-ethereum" },
        { "DOGE", "doge-dogecoin" },
        { "LTC", "ltc-litecoin" },
        { "XRP", "xrp-xrp" },
        { "BNB", "bnb-binance-coin" },
        { "SOL", "sol-solana" },
        { "ADA", "ada-cardano" },
        { "DOT", "dot-polkadot" },
        { "AVAX", "avax-avalanche" }
    };

    public async Task<List<string>> GetAvailableCryptos()
    {
        string url = "https://api.coinpaprika.com/v1/coins";

        using HttpClient client = new();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();

            var coins = JsonSerializer.Deserialize<List<CryptoRate>>(json, _jsonOptions);

            return coins
                .Where(c => c.Type == "coin" && c.Is_Active && cryptoIds.ContainsKey(c.Symbol))
                .Select(c => c.Symbol)
                .ToList();
        }

        throw new Exception("Couldn't generate crypto list");
    }

    public async Task<double> GetCryptoPriceInFiatAsync(string cryptoSymbol, string fiatCurrency)
    {
        if (!cryptoIds.ContainsKey(cryptoSymbol))
            throw new Exception("Unknown crypto symbol");

        string id = cryptoIds[cryptoSymbol];
        string url = $"https://api.coinpaprika.com/v1/tickers/{id}?quotes={fiatCurrency.ToUpper()}";

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var price = doc.RootElement
            .GetProperty("quotes")
            .GetProperty(fiatCurrency.ToUpper())
            .GetProperty("price")
            .GetDouble();

        return price;
    }
}
