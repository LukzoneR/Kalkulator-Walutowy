using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using KalkulatorWalutowy.Services;

namespace KalkulatorWalutowy;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly ExchangeRateService exchangeRateService = new ExchangeRateService();

    private string amount = "1";
    public string Amount
    {
        get => amount;
        set
        {
            if (amount != value)
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
    }

    private string selectedFrom;
    public string SelectedFrom
    {
        get => selectedFrom;
        set
        {
            if (selectedFrom != value)
            {
                selectedFrom = value;
                OnPropertyChanged(nameof(SelectedFrom));
            }
        }
    }

    private string selectedTo;
    public string SelectedTo
    {
        get => selectedTo;
        set
        {
            if (selectedTo != value)
            {
                selectedTo = value;
                OnPropertyChanged(nameof(SelectedTo));
            }
        }
    }

    private string result;
    public string Result
    {
        get => result;
        set
        {
            if (result != value)
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }
    }

    public ICommand ConvertCommand { get; }
    public ObservableCollection<string> CurrencyList { get; set; } = new();

    public MainViewModel()
    {
        ConvertCommand = new Command(async () => await ConvertAsync());
        _ = LoadCurrenciesAsync();
    }

    private async Task ConvertAsync()
    {
        if (double.TryParse(Amount, out double amountValue))
        {
            try
            {
                double rateFrom = await exchangeRateService.GetExchangeRateAsync(SelectedFrom);
                double rateTo = await exchangeRateService.GetExchangeRateAsync(SelectedTo);
                double resultValue = amountValue * (rateTo / rateFrom);
                Result = $"{resultValue:F4} {SelectedTo}";
            }
            catch (Exception ex)
            {
                Result = $"Błąd: {ex.Message}";
            }
        }
        else
        {
            Result = "Nieprawidłowa kwota.";
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

            SelectedFrom ??= "PLN";
            SelectedTo ??= "EUR";
        }
        catch (Exception ex)
        {
            Result = $"Błąd ładowania walut: {ex.Message}";
        }
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
