using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KalkulatorWalutowy;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<string> Waluty { get; set; } = new()
    {
        "PLN", "EUR", "USD", "bitcoin", "ethereum"
    };

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

    public MainViewModel()
    {
        SelectedFrom = "PLN";
        SelectedTo = "EUR";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
