using FrontMaui.Models;
using System.Net.Http.Json;

namespace FrontMaui.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("Api");
    }

    public async Task<List<Customer>> GetCustomersAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<Customer>>("/api/Customer", ct) ?? new();

    public async Task<List<Card>> GetCardsAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<Card>>("/api/Card", ct) ?? new();

    public async Task<Spend?> CreateSpendAsync(Spend spend, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("/api/Spend", spend, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Create spend failed: {(int)resp.StatusCode} {resp.ReasonPhrase} - {msg}");
        }
        return await resp.Content.ReadFromJsonAsync<Spend>(cancellationToken: ct);
    }

    public async Task UpdateSpendAsync(int idSpend, Spend spend, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync($"/api/Spend/{idSpend}", spend, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Update spend failed: {(int)resp.StatusCode} {resp.ReasonPhrase} - {msg}");
        }
    }

    public async Task DeleteSpendAsync(int idSpend, CancellationToken ct = default)
    {
        var resp = await _http.DeleteAsync($"/api/Spend/{idSpend}", ct);
        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Delete spend failed: {(int)resp.StatusCode} {resp.ReasonPhrase} - {msg}");
        }
    }
}
