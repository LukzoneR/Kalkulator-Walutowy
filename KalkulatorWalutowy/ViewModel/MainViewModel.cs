using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Globalization;

namespace KalkulatorWalutowy;

public class MainViewModel : INotifyPropertyChanged
{

    private decimal _amount;
    private string _result = "0";

    public ObservableCollection<string> Waluty { get; set; } = new()
    {
        "PLN", "EUR", "USD", "bitcoin", "ethereum"
    };

    public decimal Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnPropertyChanged();
        }
    }

    public string Result
    {
        get => _result;
        set
        {
            _result = value;
            OnPropertyChanged();
        }
    }

    private string? _selectedFrom;
    public string? SelectedFrom
    {
        get => _selectedFrom;
        set
        {
            _selectedFrom = value;
            OnPropertyChanged();
        }
    }

    private string? _selectedTo;
    public string? SelectedTo
    {
        get => _selectedTo;
        set
        {
            _selectedTo = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}