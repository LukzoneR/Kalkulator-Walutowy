using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using KalkulatorWalutowy.Services;
using KalkulatorWalutowy.ViewModel;

namespace KalkulatorWalutowy;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly ExchangeRateService exchangeRateService = new ExchangeRateService();

    public CurrencyConversionData CurrencyData { get; set; } = new();
    public CurrencyConversionData CryptoData { get; set; } = new();

    public ICommand ConvertCommand { get; }
    public ICommand CryptoConvertCommand { get; }

    public ObservableCollection<string> CurrencyList { get; set; } = new();

    public MainViewModel()
    {
        ConvertCommand = new Command(async () => await ConvertAsync());
        CryptoConvertCommand = new Command(async () => await CryptoConvertAsync());
        _ = LoadCurrenciesAsync();
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
                CurrencyData.Result = $"Błąd: {ex.Message}";
            }
        }
        else
        {
            CurrencyData.Result = "Nieprawidłowa kwota.";
        }
    }

    private async Task CryptoConvertAsync()
    {
        if (double.TryParse(CryptoData.Amount, out double amountValue))
        {
            try
            {
                double rateFrom = await exchangeRateService.GetExchangeRateAsync(CryptoData.SelectedFrom);
                double rateTo = await exchangeRateService.GetExchangeRateAsync(CryptoData.SelectedTo);
                double resultValue = amountValue * (rateFrom / rateTo);
                CryptoData.Result = $"{resultValue:F6} {CryptoData.SelectedTo}";
            }
            catch (Exception ex)
            {
                CryptoData.Result = $"Błąd: {ex.Message}";
            }
        }
        else
        {
            CryptoData.Result = "Nieprawidłowa kwota.";
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
            CryptoData.SelectedFrom ??= "PLN";
            CryptoData.SelectedTo ??= "EUR";
        }
        catch (Exception ex)
        {
            CurrencyData.Result = $"Błąd ładowania walut: {ex.Message}";
        }
    }
}
