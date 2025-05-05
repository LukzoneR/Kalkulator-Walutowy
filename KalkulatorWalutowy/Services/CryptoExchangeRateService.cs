using System.Text.Json;
using KalkulatorWalutowy.Model;

namespace KalkulatorWalutowy.Services;

public class CryptoExchangeRateService
{
    private readonly HttpClient _client = new();

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
            var coins = JsonSerializer.Deserialize<List<CryptoRate>>(json);

            return coins
                .Where(c => c.type == "coin" && c.is_active && cryptoIds.ContainsKey(c.symbol))
                .Select(c => c.symbol)
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
