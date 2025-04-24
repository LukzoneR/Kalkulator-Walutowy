using KalkulatorWalutowy.ViewModel;
namespace KalkulatorWalutowy;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}

