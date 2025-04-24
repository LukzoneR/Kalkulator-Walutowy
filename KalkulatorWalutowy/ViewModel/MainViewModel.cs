using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KalkulatorWalutowy.ViewModel;
public class MainViewModel : INotifyPropertyChanged
{
    private readonly ExchangeRateService _service = new();

    private string _selectedFrom = "PLN";
    private string _selectedTo = "EUR";
    private string _amount = "1";
    private decimal _result;

    public string SelectedFrom
    {
        get => _selectedFrom;
        set { _selectedFrom = value; OnPropertyChanged(); }
    }

    public string SelectedTo
    {
        get => _selectedTo;
        set { _selectedTo = value; OnPropertyChanged(); }
    }

    public string Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); }
    }

    public decimal Result
    {
        get => _result;
        set { _result = value; OnPropertyChanged(); }
    }


    public ICommand ConvertCommand { get; }

    public MainViewModel()
    {
        ConvertCommand = new Command(async () => await ConvertAsync());
    }


    public async Task ConvertAsync()
    {
        Console.WriteLine($"[DEBUG] Kliknięto przelicz");
        Console.WriteLine($"[DEBUG] Amount (string): {Amount}");
        Console.WriteLine($"[DEBUG] Z: {SelectedFrom}, Na: {SelectedTo}");

        try
        {
            if (!decimal.TryParse(Amount, out var amountDecimal))
            {
                Console.WriteLine("[DEBUG] Błąd konwersji Amount");
                Result = 0;
                return;
            }

            decimal rate = 5.5m;

            if (IsCrypto(SelectedFrom))
                rate = await _service.GetCryptoRateAsync(SelectedFrom.ToLower(), SelectedTo.ToLower());
            else
                rate = await _service.GetFiatRateAsync(SelectedFrom.ToUpper(), SelectedTo.ToUpper());

            Result = amountDecimal * rate;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd API: {ex.Message}");
        }
    }

    private bool IsCrypto(string currency)
    {
        var cryptoList = new[] { "bitcoin", "ethereum", "dogecoin" };
        return cryptoList.Contains(currency.ToLower());
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}