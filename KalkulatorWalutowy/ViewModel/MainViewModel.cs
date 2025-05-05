using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using KalkulatorWalutowy.Services;
using KalkulatorWalutowy.ViewModel;
using System.Runtime.CompilerServices;

namespace KalkulatorWalutowy;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly ExchangeRateService exchangeRateService = new();
    private readonly CryptoExchangeRateService cryptoExchangeRateService = new();

    public CurrencyConversionData CurrencyData { get; set; } = new();
    public CurrencyConversionData CryptoData { get; set; } = new();

    public ICommand ConvertCommand { get; }
    public ICommand CryptoConvertCommand { get; }

    public ObservableCollection<string> CurrencyList { get; set; } = new();
    public ObservableCollection<string> CryptoList { get; set; } = new();

    public MainViewModel()
    {
        ConvertCommand = new Command(async () => await ConvertAsync());
        CryptoConvertCommand = new Command(async () => await CryptoConvertAsync());

        _ = LoadCurrenciesAsync();
        _ = LoadCryptosAsync();
    }

    private async Task ConvertAsync()
    {
        if (double.TryParse(CurrencyData.Amount, out double amountValue))
        {
            try
            {
                double rateFrom = await exchangeRateService.GetExchangeRateAsync(CurrencyData.SelectedFrom);
                double rateTo = await exchangeRateService.GetExchangeRateAsync(CurrencyData.SelectedTo);
                double resultValue = amountValue * (rateFrom / rateTo);
                CurrencyData.Result = $"{resultValue:F4} {CurrencyData.SelectedTo}";
            }
            catch (Exception ex)
            {
                CurrencyData.Result = $"Error: {ex.Message}";
            }
        }
        else
        {
            CurrencyData.Result = "Wrong amount";
        }
    }

    private async Task CryptoConvertAsync()
    {
        if (double.TryParse(CryptoData.Amount, out double amountValue))
        {
            try
            {
                double cryptoToFiat = await cryptoExchangeRateService.GetCryptoPriceInFiatAsync(CryptoData.SelectedFrom, CryptoData.SelectedTo);
                double resultValue = amountValue * cryptoToFiat;
                CryptoData.Result = $"{resultValue:F2} {CryptoData.SelectedTo}";
            }
            catch (Exception ex)
            {
                CryptoData.Result = $"Error: {ex.Message}";
            }
        }
        else
        {
            CryptoData.Result = "Wrong amount.";
        }
    }

    private async Task LoadCurrenciesAsync()
    {
        try
        {
            var currencies = await exchangeRateService.GetAvailableCurrenciesAsync();
            CurrencyList.Clear();
            foreach (var currency in currencies)
            {
                CurrencyList.Add(currency);
            }

            CurrencyData.SelectedFrom ??= "PLN";
            CurrencyData.SelectedTo ??= "EUR";

            CryptoData.SelectedTo ??= "PLN"; 
        }
        catch (Exception ex)
        {
            CurrencyData.Result = $"Error currencies loading: {ex.Message}";
        }
    }


    private async Task LoadCryptosAsync()
    {
        try
        {
            var cryptos = await cryptoExchangeRateService.GetAvailableCryptos();
            CryptoList.Clear();
            foreach (var crypto in cryptos)
            {
                CryptoList.Add(crypto);
            }

            CryptoData.SelectedFrom ??= "BTC";
        }
        catch (Exception ex)
        {
            CryptoData.Result = $"Error currencies loading: {ex.Message}";
        }
    }


    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
