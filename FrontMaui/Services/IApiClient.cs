using FrontMaui.Models;

namespace FrontMaui.Services;

public interface IApiClient
{
    Task<List<Customer>> GetCustomersAsync(CancellationToken ct = default);
    Task<List<Card>> GetCardsAsync(CancellationToken ct = default);
    Task<Spend?> CreateSpendAsync(Spend spend, CancellationToken ct = default);
    Task UpdateSpendAsync(int idSpend, Spend spend, CancellationToken ct = default);
    Task DeleteSpendAsync(int idSpend, CancellationToken ct = default);
}
