namespace FrontMaui;

using FrontMaui.Helpers;
using FrontMaui.ViewModels;

public partial class MainPage : ContentPage
{
    public AddSpendViewModel ViewModel { get; }

    public MainPage()
    {
        InitializeComponent();
        ViewModel = ServiceHelper.GetService<AddSpendViewModel>();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel.Customers.Count == 0 || ViewModel.Cards.Count == 0)
        {
            await ViewModel.LoadAsync();
        }
    }
}
