namespace KalkulatorWalutowy;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue)) return;

        if (!decimal.TryParse(e.NewTextValue, out decimal value))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }
    }

    private void OnExchangeButtonClicked(object sender, EventArgs e)
    {
        var choice1 = picker1.SelectedItem.ToString();
        var choice2 = picker2.SelectedItem.ToString();
        
        picker1.SelectedItem = choice2;
        picker2.SelectedItem = choice1;
    }
}

