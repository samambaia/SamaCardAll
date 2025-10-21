using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrontMaui.Models;
using FrontMaui.Services;

namespace FrontMaui.ViewModels;

public class AddSpendViewModel : INotifyPropertyChanged
{
    private readonly IApiClient _api;
    private readonly CultureInfo _brCulture = new("pt-BR");

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<Card> Cards { get; } = new();
    public ObservableCollection<Customer> FilteredCustomers { get; } = new();
    public ObservableCollection<Card> FilteredCards { get; } = new();

    private string _customerQuery = string.Empty;
    public string CustomerQuery
    {
        get => _customerQuery;
        set { if (Set(ref _customerQuery, value)) ApplyCustomerFilter(); }
    }

    private string _cardQuery = string.Empty;
    public string CardQuery
    {
        get => _cardQuery;
        set { if (Set(ref _cardQuery, value)) ApplyCardFilter(); }
    }

    private Customer? _selectedCustomer;
    public Customer? SelectedCustomer
    {
        get => _selectedCustomer;
        set { if (Set(ref _selectedCustomer, value)) { OnPropertyChanged(nameof(CanSubmit)); } }
    }

    private Card? _selectedCard;
    public Card? SelectedCard
    {
        get => _selectedCard;
        set { if (Set(ref _selectedCard, value)) { OnPropertyChanged(nameof(CanSubmit)); } }
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set { if (Set(ref _description, value)) { OnPropertyChanged(nameof(CanSubmit)); } }
    }

    private DateTime _datePurchase = DateTime.Today;
    public DateTime DatePurchase
    {
        get => _datePurchase;
        set => Set(ref _datePurchase, value);
    }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { if (Set(ref _amount, value)) RecalculateInstallment(); }
    }

    private string _amountString = string.Empty;
    public string AmountString
    {
        get => _amountString;
        set
        {
            if (Set(ref _amountString, value))
            {
                if (decimal.TryParse(value, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, _brCulture, out var parsed))
                {
                    Amount = parsed;
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    Amount = 0m;
                }
            }
            OnPropertyChanged(nameof(CanSubmit));
        }
    }

    private int _installmentPlan = 1; // 1-60
    public int InstallmentPlan
    {
        get => _installmentPlan;
        set { if (Set(ref _installmentPlan, Math.Clamp(value, 1, 60))) RecalculateInstallment(); }
    }

    private decimal _installmentValue;
    public decimal InstallmentValue
    {
        get => _installmentValue;
        private set => Set(ref _installmentValue, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { if (Set(ref _isBusy, value)) { OnPropertyChanged(nameof(IsNotBusy)); } }
    }

    public bool IsNotBusy => !IsBusy;

    public bool CanSubmit => !IsBusy
        && !string.IsNullOrWhiteSpace(Description)
        && SelectedCustomer is not null
        && SelectedCard is not null
        && Amount > 0
        && InstallmentPlan >= 1;

    public ICommand LoadCommand { get; }
    public ICommand SubmitCommand { get; }

    public AddSpendViewModel(IApiClient api)
    {
        _api = api;
        LoadCommand = new Command(async () => await LoadAsync());
        SubmitCommand = new Command(async () => await SubmitAsync(), () => CanSubmit);
    }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Customers.Clear();
            Cards.Clear();
            var customers = await _api.GetCustomersAsync();
            foreach (var c in customers)
            {
                Customers.Add(c);
            }
            // Sort Customers after adding
            var sortedCustomers = Customers.OrderBy(c => c.CustomerName).ToList();
            Customers.Clear();
            foreach (var c in sortedCustomers)
            {
                Customers.Add(c);
            }

            var cards = await _api.GetCardsAsync();
            foreach (var c in cards)
            {
                Cards.Add(c);
            }
            // Sort Cards after adding
            var sortedCards = Cards.OrderBy(c => c.Bank).ToList();
            Cards.Clear();
            foreach (var c in sortedCards)
            {
                Cards.Add(c);
            }

            ApplyCustomerFilter();
            ApplyCardFilter();
        }
        finally
        {
            IsBusy = false;
            (SubmitCommand as Command)?.ChangeCanExecute();
        }
    }

    private void ApplyCustomerFilter()
    {
        FilteredCustomers.Clear();
        var q = (CustomerQuery ?? string.Empty).Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(q))
        {
            foreach (var c in Customers)
                FilteredCustomers.Add(c);
        }
        else
        {
            foreach (var c in Customers)
            {
                if ((c.CustomerName ?? string.Empty).ToLowerInvariant().Contains(q))
                    FilteredCustomers.Add(c);
            }
        }
    }

    private void ApplyCardFilter()
    {
        FilteredCards.Clear();
        var q = (CardQuery ?? string.Empty).Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(q))
        {
            foreach (var c in Cards)
                FilteredCards.Add(c);
        }
        else
        {
            foreach (var c in Cards)
            {
                if ((c.Bank ?? string.Empty).ToLowerInvariant().Contains(q) ||
                    (c.Number ?? string.Empty).ToLowerInvariant().Contains(q))
                    FilteredCards.Add(c);
            }
        }
    }

    private void RecalculateInstallment()
    {
        if (InstallmentPlan <= 0) 
        { 
            InstallmentValue = 0; 
            return; 
        }

        var value = Amount / InstallmentPlan;

        InstallmentValue = Math.Round(value, 2, MidpointRounding.AwayFromZero);

        OnPropertyChanged(nameof(InstallmentValueDisplay));

        (SubmitCommand as Command)?.ChangeCanExecute();
    }

    public string InstallmentValueDisplay => string.Format(_brCulture, "R$ {0:N2}", InstallmentValue);

    private async Task SubmitAsync()
    {
        if (!CanSubmit) return;
        try
        {
            IsBusy = true;
            var spend = new Spend
            {
                Expenses = Description.Trim(),
                CustomerIdCustomer = SelectedCustomer!.IdCustomer,
                CardIdCard = SelectedCard!.IdCard,
                Date = DatePurchase,
                Amount = Amount,
                InstallmentPlan = InstallmentPlan,
                InstallmentValue = InstallmentValue
            };

            await _api.CreateSpendAsync(spend);

            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage is not null)
                await mainPage.DisplayAlert("Success", "Expense saved successfully.", "OK");

            // Reset form
            Description = string.Empty;
            Amount = 0;
            AmountString = string.Empty;
            InstallmentPlan = 1;
            RecalculateInstallment();
        }
        catch (Exception ex)
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage is not null)
                await mainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
