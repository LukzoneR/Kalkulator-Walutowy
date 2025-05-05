using System.ComponentModel;

namespace KalkulatorWalutowy.ViewModel;

public class CurrencyConversionData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

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

    private string selectedFrom = string.Empty;
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

    private string selectedTo = string.Empty;
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

    private string result = string.Empty;
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

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
