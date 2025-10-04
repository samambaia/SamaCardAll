using System;
using System.Threading;
using System.Threading.Tasks;
using FrontMaui.Models;
using FrontMaui.Services;
using FrontMaui.ViewModels;
using FluentAssertions;
using Xunit;

namespace FrontMaui.Tests;

public class AddSpendViewModelTests
{
    private class FakeApiClient : IApiClient
    {
        public Task<List<Customer>> GetCustomersAsync(CancellationToken ct = default)
            => Task.FromResult(new List<Customer> { new() { IdCustomer = 1, CustomerName = "Alice" } });
        public Task<List<Card>> GetCardsAsync(CancellationToken ct = default)
            => Task.FromResult(new List<Card> { new() { IdCard = 10, Bank = "Visa", Number = "1234" } });

        public TaskCompletionSource<Spend> Created { get; } = new();

        public Task<Spend?> CreateSpendAsync(Spend spend, CancellationToken ct = default)
        {
            Created.TrySetResult(spend);
            return Task.FromResult<Spend?>(spend);
        }

        public Task UpdateSpendAsync(int idSpend, Spend spend, CancellationToken ct = default) => Task.CompletedTask;
        public Task DeleteSpendAsync(int idSpend, CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public async Task RecalculateInstallment_ComputesRoundedValue()
    {
        var fake = new FakeApiClient();
        var vm = new AddSpendViewModel(fake);
        await vm.LoadAsync();
        vm.Amount = 100m;
        vm.InstallmentPlan = 3;
        vm.InstallmentValue.Should().Be(33.33m);
        vm.InstallmentValueDisplay.Should().Contain("R$");
    }

    [Fact]
    public async Task CanSubmit_True_WhenRequiredFieldsSet()
    {
        var fake = new FakeApiClient();
        var vm = new AddSpendViewModel(fake);
        await vm.LoadAsync();
        vm.SelectedCustomer = vm.Customers[0];
        vm.SelectedCard = vm.Cards[0];
        vm.Description = "Groceries";
        vm.Amount = 120m;
        vm.InstallmentPlan = 12;
        vm.CanSubmit.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitCommand_SendsSpendToApi()
    {
        var fake = new FakeApiClient();
        var vm = new AddSpendViewModel(fake);
        await vm.LoadAsync();
        vm.SelectedCustomer = vm.Customers[0];
        vm.SelectedCard = vm.Cards[0];
        vm.Description = "Electronics";
        vm.Amount = 120m;
        vm.InstallmentPlan = 12;

        vm.SubmitCommand.Execute(null);

        var created = await fake.Created.Task.WaitAsync(TimeSpan.FromSeconds(2));
        created.Expenses.Should().Be("Electronics");
        created.CustomerIdCustomer.Should().Be(vm.SelectedCustomer!.IdCustomer);
        created.CardIdCard.Should().Be(vm.SelectedCard!.IdCard);
        created.Amount.Should().Be(120m);
        created.InstallmentPlan.Should().Be(12);
        created.InstallmentValue.Should().Be(10m);
    }
}
